# PlantCare OS: Hibrid İntellektual Agent və Simvolik Ekspert Sistemi

> **Bitkilərin differensial diaqnostikası və vital idarəetməsi üzrə intellektual ekspert sistemi**  
> *Akademik Standart: Klassik Simvolik Süni İntellekt (Russell & Norvig, Shortliffe / MYCIN, Buchanan) & Domain-Driven Design (DDD) — Python 3.11+*

---

## 💾 FLEŞKART İLƏ BAŞQA KOMPÜTERDƏ (WINDOWS) İŞƏ SALMA TƏLİMATI

Layihə **tək bir avtonom `.exe` faylına** yığılmışdır.  
Hədəf kompüterdə Python-un, kitabxanaların və ya internetin olmasına **qətiyyən ehtiyac yoxdur**.

### Nə etmək lazımdır?

#### 1-ci Variant: Cəmi 1 fayl (Ən asan yol)
1. Fleşkartınızı cari kompüterə qoşun.
2. Layihədəki **`dist/main.exe`** (və ya birbaşa qovluqdakı **`main.exe`**) faylını fleşkartınıza kopyalayın.
3. Fleşkartı müəllimin noutbukuna taxın və **`main.exe` üzərinə iki dəfə klikləyin**.
4. Proqram dərhal açılacaq!

#### 2-ci Variant: Müəllimə təhvil vermək üçün ən rahat yol (Tövsiyə olunur)
Fleşkartınıza bu 3 faylı atın:
1. **`main.exe`** — əsas proqram faylı.
2. **`run_demo.bat`** — bir kliklə etalon nümayiş rejimini açır (Monstera keysi üzrə sual-cavab, WHY və HOW ağacının tam çıxarışı).
3. **`run_on_flash_drive.bat`** — bir kliklə interaktiv sual-cavab rejimini açır.

---

## ❓ KONSOL SUALLARDAN SONRA NİYƏ BAĞLANMIR?
* Windows-da proqram iki dəfə kliklənərək açıldıqda hesabat çıxan kimi konsolun avtomatik bağlanmaması üçün proqramın sonuna xüsusi gözləmə bloku əlavə edilmişdir:
  ```
  Proqramı bağlamaq üçün Enter düyməsini basın... [Press Enter to exit]
  ```
* Diaqnostika bitdikdən sonra siz hesabatı, diaqnoz cədvəlini və müalicə protokolunu rahat oxuyursunuz. Pəncərə yalnız siz **Enter** düyməsini basdıqda bağlanır.

---

## 🧪 LAYİHƏNİ NECƏ YOXLAMAQ OLAR? (5 YOXLAMA ÜSULU)

### 1. Avtomatik Testlərin Yoxlanışı (Bütün daxili məntiq)
Kompüterdə terminalda (PowerShell və ya CMD) layihə qovluğunda bu əmri işə salın:
```powershell
py -m unittest discover tests
```
* **Nəticə**: `Ran 15 tests in 0.232s — OK` çıxmalıdır.  
* **Nə yoxlanılır**: MYCIN riyazi cəbri (sıfıra bölünmə qoruyucusu daxil olmaqla), Biliklər Bazasının 15 xəstəliyi, Truth Maintenance (faktın geri götürülməsi), Şennon entropiyası və E2E ssenarisi.

---

### 2. Etalon Demonstrasiya Rejimi (Monstera Keysi)
Müəllimə layihənin imkanlarını 10 saniyə içində əyani göstərmək üçün:
```powershell
py main.py --demo
```
*(Və ya sadəcə **`run_demo.bat`** faylına iki dəfə klikləyin)*
* **Ekranda nə görünəcək**:
  1. Monstera deliciosa növünün pasportu və mikroiqlim parametrləri.
  2. İlkin simptomun daxil edilməsi.
  3. **WHY Modulu**: Növbəti sualın NİYƏ verildiyinin bioloji əsası və gözləyən qaydalar.
  4. **Truth Maintenance Nümayişi**: Səhvən deyilmiş faktın geri götürülməsi (`retract`) və inamın qeyri-monoton yenidən hesablanması.
  5. Differensial diaqnoz cədvəli və rəngli CF şkalaları.
  6. **HOW Deduktiv Sübut Ağacı** (`rich.tree.Tree` ilə budaqlar).
  7. 3 mərhələli terapevtik müalicə protokolu.

---

### 3. Biliklər Bazasının Statik Yoxlanışı (Knowledge Linter)
Biliklər bazasının bütövlüyünü test etmək üçün:
```powershell
py main.py --lint
```
* **Nə yoxlanılır**: Qaydalarda dövri asılılıqların (tsikllərin), ziddiyyətlərin və ya tək qalmış naməlum əlamətlərin olmaması.
* **Nəticə**: `Biliklər Bazasının yoxlanış nəticəsi: 0 qeyd aşkar olundu.`

---

### 4. Müstəqil `.exe` Faylının Yoxlanışı
Fleşkarta yazacağınız faylı birbaşa yoxlamaq üçün:
* Qovluqdakı **`dist\main.exe`** üzərinə iki dəfə klikləyin.
* Konsol pəncərəsi açılacaq və interaktiv sorğu başlayacaq.

---

### 5. İnteraktiv Sorğu Rejimi (İstifadəçi kimi əllə yoxlama)
İstənilən bitki üzrə canlı diaqnostika keçmək üçün:
```powershell
py main.py
```
*(Və ya **`run_on_flash_drive.bat`** üzərinə iki dəfə klikləyin)*

**İdarəetmə qaydası**:
1. Bitkini seçin (məsələn, Monstera üçün `1`, Fikus üçün `2`).
2. Suallara rəqəmlərlə cavab verin:
   * `1` — Bəli, dəqiq (+1.0)
   * `2` — Çox güman (+0.6)
   * `3` — Əmin deyiləm (0.0)
   * `4` — Çox güman xeyr (-0.6)
   * `5` — Qətiyyən xeyr (-1.0)
3. Əlavə komandalar:
   * `why` — Sualın nə üçün verildiyini göstərir.
   * `status` — Cari hipotezlərin cədvəlini çıxarır.
   * `retract <simptom_id>` — Əvvəl verilmiş faktı geri götürür.
   * `skip` — Sualı neytral olaraq keçir.
   * `finish` — Sorğunu vaxtından əvvəl tamamlayıb diaqnozu çıxarır.

---

## 🌿 SİSTEMİN ƏHATƏ DAİRƏSİ

### Dəstəklənən 5 Bitki Növü:
1. `monstera_deliciosa` (Monstera deliciosa / Dəlikli monstera)
2. `ficus_benjamina` (Ficus benjamina / Benyamin fikusu)
3. `sansevieria_trifasciata` (Sansevieria trifasciata / Qayınana dili)
4. `spathiphyllum_wallisii` (Spathiphyllum wallisii / Qadın xoşbəxtliyi)
5. `phalaenopsis` (Phalaenopsis / Orxideya Falenopsis)

### Təyin Edilən 15 Patologiya və Aqrotexniki Problem:
1. **Kök çürüməsi** (*Anaerobic root necrobiosis / Pythium spp.*)
2. **Kəskin su çatışmazlığı** (*Desiccatio acuta / Plazmoliz*)
3. **Etiolyasiya** (*Etiolatio vegetativa / İşıq aclığı*)
4. **Fotodestruksiya** (*Günəş yanığı / UB yanıq*)
5. **Hörümçək gənəsi** (*Tetranychidae zədələnməsi*)
6. **Unlu yastıca** (*Pseudococcidae*)
7. **Tripslər** (*Thysanoptera*)
8. **Çanaqlı yastıca / Yalançı çanaqlı yastıca** (*Coccidae*)
9. **Dəmir xlorozi** (*Chlorosis ferrica / Fe çatışmazlığı*)
10. **Azot çatışmazlığı** (*Deficientia nitrogenii*)
11. **Kökün osmotik yanığı** (*Duz toksikozu*)
12. **Bakterial ləkələnmə** (*Xanthomonas / Pseudomonas*)
13. **Quru hava / Aşağı VPD** (*Yarpaq uclarının quruması*)
14. **Temperatur şoku** (*Soyuq cərəyan / Qəfil yarpaq tökülməsi*)
15. **Ödəm / Su şişkinliyi** (*Edema*)

---

## 📐 RİYAZİ VƏ NƏZƏRİ ƏSASLAR (MÜƏLLİMƏ İZAH ÜÇÜN)

1. **MYCIN Əminlik Əmsalları Cəbri (Shortliffe / Buchanan)**:
   * Konyunksiya (AND): $CF = \min(CF_1, \dots, CF_n)$
   * Dizyunksiya (OR): $CF = \max(CF_1, \dots, CF_n)$
   * Qaydanın aktivləşməsi: $CF(h, e) = CF_{rule} \times \max(0, CF(e))$
   * Müstəqil dəlillərin toplanması:
     * Hər ikisi müsbət ($CF_1, CF_2 \ge 0$): $CF = CF_1 + CF_2 \cdot (1 - CF_1)$
     * Hər ikisi mənfi ($CF_1, CF_2 \le 0$): $CF = CF_1 + CF_2 \cdot (1 + CF_1)$
     * Əks işarəli olduqda: $CF = \frac{CF_1 + CF_2}{1 - \min(|CF_1|, |CF_2|)}$
     * Mütləq ziddiyyət ($+1.0$ və $-1.0$): Sıfıra bölünmə qoruyucusu ilə avtomatik $0.0$ qəbul edilir.
2. **Adaptiv Sual Seçimi (Expected Information Gain — EIG)**:
   Sual ardıcıl yox, Şennon entropiyasının azalması meyarı ilə seçilir:
   $$Q^* = \arg\max_{Q} \left[ H(Hypotheses) - \sum_{a \in \{yes, no\}} P(a \mid Q) \cdot H(Hypotheses \mid Q = a) \right]$$
3. **İzaholunan Süni İntellekt (Explainable AI - XAI)**:
   * **WHY**: Sualın nə üçün verildiyini göstərir.
   * **HOW**: Nəticənin necə alındığını göstərən deduktiv ağac qurur.
4. **Qeyri-monoton Məntiq (Truth Maintenance)**:
   İstifadəçi səhv etdikdə `retract` əmri ilə fakt ləğv edilir və bütün hipotezlər sıfırdan avtomatik yenidən hesablanır.
