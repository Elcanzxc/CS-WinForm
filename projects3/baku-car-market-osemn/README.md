# 🚗 Baku Auto Market CDSS (Car Decision Support System)
### *Bakı İkinci Əl Avtomobil Bazarının Analitikası və Qiymət Proqnozu*
### *Аналитика и Прогноз Цен Вторичного Авторынка Баку*

**Metodologiya / Методология:** OSEMN (Obtain, Scrub, Explore, Model, Interpret)  
**Texnoloji Stek / Стек:** Python 3.10+, Streamlit, Scikit-learn, Plotly, Pandas, NumPy, Joblib  
**Dillər / Языки документации:** 🇷🇺 Русский | 🇦🇿 Azərbaycan dili  

---

## 📑 Mündəricat / Содержание
1. [🇷🇺 РУССКИЙ: Описание проекта](#-русский-руководство-пользователя)
   - [Архитектура и структура](#-архитектура-файлов)
   - [Инструкция: Запуск на другом ноутбуке с флешки](#-как-запустить-проект-на-другом-ноутбуке-с-флешки)
   - [Пошаговая проверка функционала (Checklist)](#-пошаговая-проверка-проекта)
   - [Этапы OSEMN и Machine Learning](#-реализация-методологии-osemn)
2. [🇦🇿 AZƏRBAYCAN DİLİ: Layihə Bələdçisi](#-azərbaycan-dili-layihə-təlimatı)
   - [Fayl Strukturu](#-fayl-strukturu)
   - [Fleşkadan Başqa Noutbukda Başlatma Təlimatı](#-layihənin-fleşkadan-digər-noutbukda-işə-salınması)
   - [Addım-addım Yoxlama Təlimatı](#-funksionallığın-yoxlanılması)
   - [OSEMN Metodologiyası və Nəticələr](#-osemn-mərhələləri-və-maşın-öyrənməsi)

---

# 🇷🇺 РУССКИЙ: РУКОВОДСТВО ПОЛЬЗОВАТЕЛЯ

## 📌 Описание проекта
**Baku Auto Market CDSS** — это комплексный Data Science проект полного цикла (End-to-End), разработанный по методологии **OSEMN**. Проект исследует специфику вторичного авторынка Азербайджана (аналог платформ **Turbo.az** и **Tap.az**), обучает современные модели машинного обучения для предсказания рыночной стоимости автомобиля в AZN/USD и предоставляет интеллектуальную систему поддержки принятия решений (CDSS) через веб-дашборд на **Streamlit** и академический **Jupyter Notebook**.

---

## 📂 Архитектура файлов
```text
baku-car-market-osemn/
├── data/
│   ├── raw_cars_baku.csv              # Исходный сырой датасет (4,538 объявлений)
│   └── cleaned_cars_baku.csv          # Очищенный датасет после этапа Scrub (4,244 авто)
├── models/
│   └── baku_car_model.joblib          # Сохраненная лучшая обученная модель (Random Forest)
├── notebooks/
│   └── baku_cars_osemn_pipeline.ipynb # Академический Jupyter Notebook со всеми 5 шагами
├── src/
│   ├── obtain.py                      # Сбор данных: парсер + генератор бакинского рынка
│   ├── scrub.py                       # Очистка: Regex, конвертация USD->AZN, IQR выбросы
│   ├── explore.py                     # EDA: интерактивные Plotly графики в Dark Fintech теме
│   ├── model.py                       # Обучение моделей: Ridge, Random Forest, HistGradientBoosting
│   └── interpret.py                   # Feature Importance и бизнес-интерпретация CDSS
├── app.py                             # Интерактивный веб-дашборд на Streamlit (5 вкладок)
├── requirements.txt                   # Зависимости Python
├── start.bat                          # Скрипт запуска в 1 клик для Windows (включая флешку)
└── README.md                          # Документация (Русский / Азербайджанский)
```

---

## 💾 КАК ЗАПУСТИТЬ ПРОЕКТ НА ДРУГОМ НОУТБУКЕ С ФЛЕШКИ

Проект спроектирован так, чтобы работать **максимально автономно и портативно** на любом компьютере под управлением Windows.

### 📋 Что должно быть на целевом ноутбуке:
1. Установлен **Python 3.10, 3.11, 3.12 или 3.13** с официального сайта [python.org](https://www.python.org/).
   > ⚠️ **КРИТИЧЕСКИ ВАЖНО при установке Python:** на первом же экране инсталлятора обязательно поставьте галочку **"Add python.exe to PATH"** (Добавить Python в PATH).
2. Доступ к интернету на 1–2 минуты при самом первом запуске (чтобы pip скачал библиотеки из `requirements.txt`). После этого проект работает **полностью офлайн**.

---

### 🚀 Пошаговый запуск с флешки:

#### Шаг 1. Подготовка флешки
Скопируйте всю папку `baku-car-market-osemn` на вашу флешку.
> 💡 *Совет:* Папку виртуального окружения `.venv` (если она уже есть) копировать не обязательно — скрипт `start.bat` сам корректно пересоздаст её под операционную систему нового ноутбука.

#### Шаг 2. Подключение к новому ноутбуку
Вставьте флешку в целевой ноутбук. Зайдите на флешку и откройте папку `baku-car-market-osemn`.

#### Шаг 3. Запуск в один клик
Дважды щелкните по файлу:
```text
start.bat
```
*(Или в корне проекта: `start.bat`)*.

#### Что автоматически сделает `start.bat`:
1. Сам определит букву диска флешки (например, `E:`, `F:`, `D:`) благодаря команде `cd /d "%~dp0"`.
2. Проверит наличие Python в системе (`py` или `python`).
3. Проверит виртуальное окружение `.venv`. Если проект был скопирован с другого ноутбука и старый venv не подходит — скрипт **автоматически очистит старый и создаст свежий `.venv`**.
4. Установит необходимые зависимости из `requirements.txt` (`streamlit`, `pandas`, `scikit-learn`, `plotly`, `joblib`).
5. Проверит наличие датасета и модели. Если датасет еще не сгенерирован — автоматически выполнит конвейер `obtain -> scrub -> model`.
6. Запустит веб-сервер Streamlit и **автоматически откроет браузер** по адресу:
   ```text
   http://localhost:8501
   ```

---

### ⌨️ Альтернативный ручной запуск через терминал (PowerShell / CMD)
Если вы хотите запустить проект вручную:
```powershell
# 1. Перейдите в папку проекта на флешке (например, диск E:)
cd /d E:\baku-car-market-osemn

# 2. Создайте и активируйте окружение
py -m venv .venv
.venv\Scripts\activate

# 3. Установите зависимости
pip install -r requirements.txt

# 4. Запустите дашборд
streamlit run app.py
```

---

## 🔍 ПОШАГОВАЯ ПРОВЕРКА ПРОЕКТА

После запуска в браузере откроется веб-платформа. Вот чек-лист для быстрой проверки всех компонентов:

### 1. Проверка вкладки `[O] Məlumatların Bazası` (База данных)
- В верхней части отображаются 5 карточек KPI (Всего объявлений: ~4,538, Чистая база: ~4,244, Медианная цена, Бренд-лидер, Точность $R^2$).
- Проверьте таблицу сырых объявлений: там видны цены как в AZN, так и в USD (`$18 000`), единицы измерения (`185 000 km`, `2.0 L`).
- Проверьте круговую диаграмму валют (~35% USD, ~65% AZN).
- Нажмите кнопку **"Bazanı Yenilə / Yenidən Sintez Et"** — база мгновенно обновится без зависаний.

### 2. Проверка вкладки `[S] Təmizlənmə Pipelin-i` (Очистка и Scrub)
- Отображаются карточки аудита: удаленные дубликаты (~35 шт.), устраненные выбросы (IQR Outliers).
- Сравнение таблиц «До» (текстовые единицы) и «После» (числовые значения, новые признаки).
- Проверьте наличие новых колонок: `Car_Age`, `Km_Per_Year`, `Is_Baku`, `Is_Damaged`.

### 3. Проверка вкладки `[E] İnteraktiv Bazar Analitikası` (EDA графики)
- Раскройте фильтры и выберите конкретную марку (например, *Toyota* или *Mercedes-Benz*) или диапазон годов.
- Проверьте 6 графиков Plotly:
  1. Топ-10 марок и их медианная цена.
  2. Кривая амортизации: бензин vs гибрид vs дизель (гибриды дешевеют медленнее).
  3. Scatter Plot: пробег против цены с пузырьками объема двигателя.
  4. Динамика эко-транспорта (рост доли гибридов и электромобилей 2018–2025).
  5. Матрица корреляции признаков (Heatmap).
  6. Распределение цен по типам кузова (Седан, SUV).

### 4. Проверка вкладки `[M] Canlı Qiymət Kalkulyatoru` (Калькулятор цен)
- В таблице сверху представлены результаты 3 алгоритмов (Ridge, Random Forest, HistGradientBoosting).
- В форме слева выберите:
  - Марка: **Toyota**, Модель: **Camry**, Год: **2019**, Пробег: **110 000 km**, Двигатель: **2.5 L**, Топливо: **Benzin**.
- Посмотрите на результат справа:
  - Прогнозируемая рыночная цена в **AZN** и в **USD**.
  - 95% доверительный интервал (± погрешность модели).
- В поле **"Deal Checker"** введите цену продавца (например, если продавец просит 32 000 AZN при рыночной 28 000 AZN — индикатор подсветится красным: *"⚠️ QİYMƏT ŞİŞİRDİLİБ"*).

### 5. Проверка вкладки `[N] Ekspert Nəticələri və Qərarlar` (CDSS Выводы)
- Отображается график **Feature Importance** (Год выпуска ~45%, Объем двигателя ~20%, Пробег ~14%).
- Изучите 5 бизнес-рекомендаций для рынка Баку.
- Нажмите кнопку **"📥 CDSS Analitik Hesabatını Yüklə (.TXT)"** для скачивания отчета.

### 6. Проверка Jupyter Notebook:
Для проверки академического ноутбука запустите в терминале:
```bash
jupyter notebook notebooks/baku_cars_osemn_pipeline.ipynb
```
Все ячейки выполняются последовательно с подробными формулами ($R^2$, $MAE$, $RMSE$, $IQR$) и пояснениями.

---

## 📊 РЕАЛИЗАЦИЯ МЕТОДОЛОГИИ OSEMN

| Этап OSEMN | Модуль | Описание и методы |
| :--- | :--- | :--- |
| **[O] Obtain** | `src/obtain.py` | Сбор данных и генерация бакинского рынка (4,538 записей). |
| **[S] Scrub** | `src/scrub.py` | Regex-парсинг, курс USD=1.70, удаление выбросов IQR, генерация фичей. |
| **[E] Explore** | `src/explore.py` | 6 интерактивных графиков Plotly в темной теме. |
| **[M] Model** | `src/model.py` | Обучение Ridge ($R^2=74.6\%$), HistGradientBoosting ($R^2=84.5\%$) и Random Forest ($R^2=85.7\%$, MAE=1,838 AZN). |
| **[N] Interpret** | `src/interpret.py` | Анализ важности признаков и бизнес-стратегии для дилеров и покупателей. |

---
---

# 🇦🇿 AZƏRBAYCAN DİLİ: LAYİHƏ TƏLİMATI

## 📌 Layihə Haqqında
**Baku Auto Market CDSS** — beynəlxalq **OSEMN** metodologiyası əsasında hazırlanmış, Bakı və Azərbaycanın ikinci əl avtomobil bazarını (**Turbo.az** / **Tap.az** analoqu) analiz edən və maşın öyrənməsi ilə avtomobilin ədalətli bazar qiymətini proqnozlaşdıran qərar dəstək sistemidir (CDSS).

---

## 📂 Fayl Strukturu
- `data/raw_cars_baku.csv` — İlkin xam məlumat bazası (4,538 elan).
- `data/cleaned_cars_baku.csv` — Təmizlənmiş və IQR filtrlərindən keçmiş dataset (4,244 sətir).
- `models/baku_car_model.joblib` — Yaddaşda saxlanılmış ən yaxşı reqressiya modeli (Random Forest).
- `notebooks/baku_cars_osemn_pipeline.ipynb` — Akademik Jupyter Notebook (5 addımlıq tədqiqat).
- `src/` — OSEMN boru kəmərinin mənbə kodları (`obtain.py`, `scrub.py`, `explore.py`, `model.py`, `interpret.py`).
- `app.py` — Streamlit veb-interfeysi (Dark Fintech UI).
- `start.bat` — Windows üçün bir kliklə işə salma skripti.
- `requirements.txt` — Kitabxanalar siyahısı.

---

## 💾 LAYİHƏNİN FLEŞKADAN DİGƏR NOUTBUKDA İŞƏ SALINMASI

Layihə elə konfiqurasiya edilmişdir ki, fleşka vasitəsilə başqa istənilən Windows kompüterində birbaşa açıla bilsin.

### 📋 Hədəf noutbuk üçün tələblər:
1. Sistemdə **Python 3.10 və ya daha yuxarı** versiya quraşdırılmalıdır ([python.org](https://www.python.org/)).
   > ⚠️ **ƏSAS QEYD:** Python-u quraşdırarkən ilk pəncərədə **"Add python.exe to PATH"** xanasını mütləq işarələyin.
2. İlk dəfə işə salarkən kitabxanaların (`requirements.txt`) yüklənməsi üçün 1 dəqiqəlik internet bağlantısı kifayətdir. Sonrakı iş tamamilə **oflayn** rejimdə gedir.

---

### 🚀 Addım-addım Fleşkadan Başlatma:

#### 1. Fleşkanın hazırlanması
`baku-car-market-osemn` qovluğunu fleşka kartınıza köçürün.

#### 2. Digər noutbuka qoşulma
Fleşkanı digər noutbuka taxın və fleşka daxilindəki `baku-car-market-osemn` qovluğuna daxil olun.

#### 3. Bir kliklə işə salma
Qovluqdakı bu fayla iki dəfə klikləyin:
```text
start.bat
```

#### `start.bat` skripti nə edir:
1. Fleşkanın disk hərfini (`E:`, `F:`, `D:`) avtomatik müəyyənləşdirir.
2. Sistemdə Python-un olmasını yoxlayır.
3. Əgər köhnə kompüterdən qalma `.venv` mühiti varsa, onun cari kompüterlə uyğunsuzluğunu aşkar edib **avtomatik təmizləyir və yeni `.venv` mühiti yaradır**.
4. Bütün lazımi kitabxanaları avtomatik quraşdırır (`pip install -r requirements.txt`).
5. İlkin məlumat bazasını və modeli yoxlayır.
6. Streamlit serverini işə salır və brauzerdə avtomatik açır:
   ```text
   http://localhost:8501
   ```

---

### ⌨️ Əl ilə (Terminal vasitəsilə) Başlatma
Əgər CMD və ya PowerShell vasitəsilə açmaq istəsəniz:
```powershell
cd /d E:\baku-car-market-osemn
py -m venv .venv
.venv\Scripts\activate
pip install -r requirements.txt
streamlit run app.py
```

---

## 🔍 FUNKSİONALLIĞIN YOXLANILMASI

Veb tətbiq brauzerdə açıldıqdan sonra aşağıdakı addımlarla yoxlayın:

1. **[O] Məlumatların Bazası:**
   - Yuxarıdakı 5 KPI kartını (4,538 xam elan, median qiymət, lider marka) nəzərdən keçirin.
   - Xam cədvəldə həm AZN, həm USD qiymətlərinin olduğunu yoxlayın.
   - "Bazanı Yenilə" düyməsi ilə canlı yenilənməni test edin.

2. **[S] Təmizlənmə Pipelin-i:**
   - Scrub hesabatını, silinmiş dublikat və kənar qiymətlərin (outliers) statistikasını yoxlayın.
   - Əvvəl və Sonra cədvəllərində `Car_Age` və `Km_Per_Year` sütunlarının yaradıldığını görün.

3. **[E] İnteraktiv Bazar Analitikası:**
   - Marka və il filtrlərini dəyişərək 6 Plotly qrafikini (Top markalar, Amortizasiya əyrisi, Korrelyasiya xəritəsi) analiz edin.

4. **[M] Canlı Qiymət Kalkulyatoru:**
   - Toyota Camry və ya Mercedes E-Class seçin, il və yürüş daxil edin.
   - Modelin proqnozlaşdırdığı **Bazar Qiyməti (AZN və USD)** və **95% Güvən İntervalını** yoxlayın.
   - "Deal Checker" bölməsində satıcının qiymətini yazıb təklifin sərfəliliyini qiymətləndirin.

5. **[N] Ekspert Nəticələri və Qərarlar:**
   - Faktorların əhəmiyyət dərəcəsi (Feature Importance) qrafikini və Bakı bazarı üçün 5 iqtisadi tövsiyəni oxuyun.
   - Analitik hesabatı `.txt` formatında yükləyin.

6. **Jupyter Notebook-un yoxlanılması:**
   ```bash
   jupyter notebook notebooks/baku_cars_osemn_pipeline.ipynb
   ```
   Notebook daxilində bütün 5 mərhələ riyazi düsturlarla ardıcıl icra üçün tam hazırdır.

---

## 👥 Müəllif və Lisenziya
- **Müəllif:** Baku Auto Market Data Science Research Team
- **Lisenziya:** MIT License
