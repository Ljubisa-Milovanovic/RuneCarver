# Git setup za RuneCarver

Ovaj repo koristi **Git LFS** za binarne fajlove i **Unity SmartMerge (UnityYAMLMerge)** za
merge scena i prefaba. Oba se konfigurišu **po mašini** — `.gitattributes` je u repou, ali
`git config` nije, pa svako mora da odradi setup ispod jednom.

Bez ovog setupa git i dalje radi, ali **tiho**:

- bez LFS-a → binarni fajlovi ulaze u istoriju kao sirovi blobovi i repo raste zauvek
- bez merge drivera → git merge-uje `.unity` i `.prefab` kao običan tekst i lako pojede tuđe izmene

Unity verzija projekta: **6000.5.4f1** (`ProjectSettings/ProjectVersion.txt`).

---

## 1. Git LFS

Instalacija:

| OS | Komanda |
|---|---|
| Linux (Debian/Ubuntu) | `sudo apt install git-lfs` |
| macOS | `brew install git-lfs` |
| Windows | uz [Git for Windows](https://git-scm.com/download/win), ili `winget install GitHub.GitLFS` |

Zatim **jednom po korisniku**:

```bash
git lfs install
```

Provera da je aktivno:

```bash
git lfs version          # mora da ispiše verziju
git config --get filter.lfs.process   # mora: git-lfs filter-process
git lfs ls-files | wc -l # mora da vrati broj > 0 u kloniranom repou
```

### Ako si već klonirao pre nego što si instalirao LFS

```bash
git lfs install
git lfs pull
```

### Provera da nisi slučajno ubacio binarni fajl mimo LFS-a

Ovo se već desilo jednom u ovom repou (`Assets/Resources/Images/Rune Carver Logo-selection.png`,
4.4 MB sirovog PNG-a u istoriji) — fajl je dodat sa mašine bez `git lfs install`, pa ga
`.gitattributes` nije uhvatio. Pre push-a proveri:

```bash
git diff --cached --stat    # novi binarni fajl treba da bude ~130 bajta (pointer), ne MB
```

Ako vidiš pravu veličinu umesto ~130 B, fajl nije prošao kroz LFS. Popravka **pre commit-a**:

```bash
git add --renormalize <putanja>
```

Šta ide u LFS definisano je u `.gitattributes` (slike, audio, video, 3D modeli, fontovi, DLL-ovi,
arhive). Ne diraj taj fajl bez dogovora — sinhronizovan je sa zvaničnim Unity template-om.

---

## 2. UnityYAMLMerge (SmartMerge)

`.gitattributes` već govori gitu da za `.unity`, `.prefab`, `.asset`, `.mat`, `.meta` i ostale
Unity YAML fajlove koristi merge driver `unityyamlmerge`. Taj driver treba definisati lokalno.

### 2a. Putanja do alata

Alat dolazi sa Unity Editorom:

| OS | Putanja |
|---|---|
| Linux | `~/Unity/Hub/Editor/6000.5.4f1/Editor/Data/Tools/UnityYAMLMerge` |
| Windows | `C:\Program Files\Unity\Hub\Editor\6000.5.4f1\Editor\Data\Tools\UnityYAMLMerge.exe` |
| macOS | `/Applications/Unity/Hub/Editor/6000.5.4f1/Unity.app/Contents/Tools/UnityYAMLMerge` |

Putanja sadrži verziju Unity-ja, pa **puca na svakom Unity update-u**. Na Linux/macOS je zato
praktično napraviti stabilan simlink i njega koristiti u configu:

```bash
ln -sfn "$HOME/Unity/Hub/Editor/6000.5.4f1/Editor/Data/Tools/UnityYAMLMerge" "$HOME/.local/bin/UnityYAMLMerge"
```

Posle update-a Unity-ja samo ponovo pokreneš istu komandu sa novom verzijom — git config ostaje isti.

### 2b. Fallback alat

SmartMerge sam rešava sve što može, a ono što ne može predaje običnom 3-way merge alatu.
Napravi `~/.config/unityyamlmerge/fallback.txt` (bilo gde, samo da nije u Unity instalaciji —
Unity update briše svoj `mergespecfile.txt`):

```
# %l = lokalna verzija, %r = incoming, %b = base, %d = izlazni fajl
# Prvi alat koji postoji na disku se koristi -> redosled je prioritet.

# VS Code merge editor
* use "/snap/bin/code" --wait --merge "%l" "%r" "%b" "%d"
* use "/usr/bin/code" --wait --merge "%l" "%r" "%b" "%d"

# Meld (sudo apt install meld)
* use "/usr/bin/meld" --auto-merge "%l" "%b" "%r" --output "%d"

# KDiff3 (sudo apt install kdiff3)
* use "/usr/bin/kdiff3" "%b" "%l" "%r" -o "%d"
```

Na Windowsu iste linije, samo sa Windows putanjama, npr.:

```
* use "C:\Users\<ti>\AppData\Local\Programs\Microsoft VS Code\Code.exe" --wait --merge "%l" "%r" "%b" "%d"
```

### 2c. Git config

```bash
# merge driver — koristi ga `git merge` automatski, preko .gitattributes
git config --global merge.unityyamlmerge.name "Unity SmartMerge"
git config --global merge.unityyamlmerge.driver '~/.local/bin/UnityYAMLMerge merge -h -p --force --fallback none %O %B %A %A'
git config --global merge.unityyamlmerge.recursive binary

# mergetool — za ručno rešavanje onoga što driver nije mogao
git config --global merge.tool unityyamlmerge
git config --global mergetool.unityyamlmerge.trustExitCode false
git config --global mergetool.unityyamlmerge.cmd '~/.local/bin/UnityYAMLMerge merge -p --force --fallback ~/.config/unityyamlmerge/fallback.txt "$BASE" "$REMOTE" "$LOCAL" "$MERGED"'
git config --global mergetool.keepBackup false
```

Detalji koji **nisu opcioni** (bez njih driver tiho gubi izmene):

- **`--force`** — git driveru prosleđuje temp fajlove bez ekstenzije (`merge_file_MPGAcA`).
  Bez `--force` alat ne prepozna tip fajla, odustane, i u fajlu ostane samo tvoja strana
  **bez ijednog conflict markera**.
- **redosled `%O %B %A %A`** — usage je `<base> <left=theirs> <right=mine> <dest>`.
  Zamena `%A` i `%B` obrne strane pri rešavanju konflikata.
- **`recursive = binary`** — sprečava da git kod kriz-merge-ova provuče "virtuelnog pretka"
  kroz isti put.
- **`--fallback none` u driveru** — `git merge` mora da ostane neinteraktivan; GUI alat se
  otvara tek kad ti sam pokreneš `git mergetool`.

Na Windowsu vrednosti su iste, samo se putanja menja (u `cmd` koristi `"$BASE"` itd. kako jeste —
git ih prosleđuje kroz shell).

---

## 3. Kako izgleda merge u praksi

```bash
git merge <grana>
```

- **Nema konflikta** → SmartMerge je spojio izmene po objektima u sceni. Izmene na različitim
  GameObject-ima se spajaju čisto, čak i kad su u istoj sceni.
- **`CONFLICT (content)`** → obe strane su menjale **isto polje**. Driver ispiše šta se sudara:

  ```
  Conflicts:
  Left  100.GameObject.m_Name change to AlphaSideA
  Right 100.GameObject.m_Name change to AlphaSideB
  ```

> **Pažnja:** kod Unity YAML fajlova **nema `<<<<<<<` markera u fajlu.** Fajl izgleda "uredno"
> ali sadrži samo jednu stranu. Nikad ne radi `git add` na konfliktnoj sceni/prefabu pre nego
> što pokreneš:

```bash
git mergetool     # otvara fallback alat sa tri verzije
git commit
```

Ako `git mergetool` ispiše `Couldn't locate merge tool to handle extension`, znači da nijedan alat
iz tvog `fallback.txt` ne postoji na disku — instaliraj neki (`sudo apt install meld`) ili ispravi
putanju.

---

## 4. Provera da setup radi

```bash
git config --get merge.unityyamlmerge.driver     # mora da ispiše komandu sa --force
git check-attr merge Assets/Scenes/SampleScene.unity   # mora: merge: unityyamlmerge
git lfs ls-files | head                                # mora da izlista binarne fajlove
```

---

## 5. Unity Editor podešavanja

Već su podešena u repou, ali ako menjaš `ProjectSettings`, ne diraj:

- **Edit → Project Settings → Editor → Asset Serialization: Force Text**
  (`m_SerializationMode: 2`) — bez ovoga scene su binarne i merge je nemoguć.
- **Version Control → Mode: Visible Meta Files** — `.meta` fajlovi se commit-uju uz asset.
  Asset i njegov `.meta` uvek idu u **isti commit**, inače se GUID-ovi razilaze i reference pucaju.
