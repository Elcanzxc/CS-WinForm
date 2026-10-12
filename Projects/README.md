# MedTriage Pro — Enterprise Klinik Qəbul, Ekspert Diaqnostika və Təcili Həyəcan Terminalı
*(Корпоративная медицинская система клинического триажа, приёма и экстренной диспетчеризации)*

---

## 📑 Mündəricat / Содержание
1. [AZƏRBAYCAN DİLİNDƏ RƏSMİ BƏLƏDÇİ](#-azərbaycan-dilində-rəsmi-bələdçi)
   - [Layihə nədir və nə üçün lazımdır?](#1-layihə-nədir-və-nə-üçün-lazımdır)
   - [Sistem necə işləyir?](#2-sistem-necə-işləyir)
   - [Bütün funksiyaların addım-addım test edilməsi](#3-bütün-funksiyaların-addım-addım-test-edilməsi)
   - [Fleşkart vasitəsilə istənilən kompüterdə 1-kliklə işə salma təlimatı](#4-fleşkart-vasitəsilə-istənilən-kompüterdə-1-kliklə-işə-salma)
2. [ИНСТРУКЦИЯ НА РУССКОМ ЯЗЫКЕ](#-инструкция-на-русском-языке)
   - [Что это за проект и зачем он нужен?](#1-что-это-за-проект-и-зачем-он-нужен)
   - [Как устроена логика и архитектура?](#2-как-устроена-логика-и-архитектура)
   - [Пошаговый сценарий тестирования всех функций](#3-пошаговый-сценарий-тестирования-всех-функций)
   - [Как запустить проект с флешки на любом стороннем ноутбуке](#4-как-запустить-проект-с-флешки-на-любом-ноутбуке)

---

# 🇦🇿 AZƏRBAYCAN DİLİNDƏ RƏSMİ BƏLƏDÇİ

## 1. Layihə nədir və nə üçün lazımdır?

Təsəvvür edin ki, xəstəxananın Təcili Qəbul (Emergency / Qəbul Şöbəsi) zalına eyni anda 15 xəstə daxil olur. Biri sadəcə soyuqlayıb və öskürür, digərində isə infarktın ilkin əlamətləri var, lakin o, sakitcə oturub növbəsini gözləyir. Əgər canlı növbə prinsipi tətbiq olunarsa («kim birinci gəlib, o girsin»), infarkt keçirən xəstə növbədə dünyasını dəyişə bilər.

**MedTriage Pro** bu problemi aradan qaldırmaq üçün yaradılmış **Ağıllı Klinik Triaj Terminalıdır**. 
Sistem beynəlxalq **Manchester Triage System (MTS)** alqoritmlərini rəhbər tutaraq xəstəni növbəyə görə deyil, **həyati təhlükə dərəcəsinə** görə sıralayır:
- **Level 1 (Qırmızı):** Reanimasiya, dərhal müdaxilə (0 dəqiqə).
- **Level 2 (Narıncı):** Çox təcili (10 dəqiqə ərzində).
- **Level 3 (Sarı):** Təcili (60 dəqiqə ərzində).
- **Level 4 (Yaşıl):** Standart qəbul (120 dəqiqə ərzində).
- **Level 5 (Mavi):** Planlı profilaktik qəbul.

---

## 2. Sistem necə işləyir?

1. **İstifadəçi üçün rahat yönləndirmə (3 Addımlı Master):**
   - **Addım 1:** Pasiyentin FİN kodu, adı, soyadı, yaşı, əlaqə telefonu və xüsusilə **Dərman Allergiyası** qeyd olunur.
   - **Addım 2:** Vital göstəricilər daxil edilir: Təzyiq (SYS/DIA), Nəbz, Oksigen (SpO2), Hərarət, Qlükoza, Ağrı şkalası (VAS 0–10). Sistem dərhal **Şok İndeksini (Nəbz/SYS)** hesablayır.
   - **Addım 3:** Kateqoriyalara bölünmüş şikayətlər seçilir (Ürək-damar, Tənəffüs, FAST insult protokolu, Qarın boşluğu, Travma/Şok).
2. **Klinik Ekspert Mühərriki (CDSS):**
   Real vaxt rejimində 17 qaydanı təhlil edir, ilkin XBT-10 diaqnozunu təyin edir, məsul həkim və kabineti seçir, təcili STAT analizləri (EKQ, KT, Troponin) müəyyənləşdirir.
3. **Avtomatik Həyəcan Siqnalı və Web Audio:**
   Əgər SpO2 < 90% və ya təzyiq > 190 olarsa, xarici mp3 fayllar olmadan birbaşa brauzerin audio prosessoru vasitəsilə Code Blue (1000 Hz) və ya Code Red (880/440 Hz) sirenaları işə düşür və bütün ekranda qırmızı çərçivə yanır.
4. **Həkim Kabinetləri və Növbə:**
   Həkim kabinetdə «Növbəti Pasiyenti Çağır» düyməsini sıxdıqda sistem növbədə ən ağır vəziyyətdə olan pasiyenti avtomatik çağırır və akustik xəstəxana qonqu səslənir.
5. **Portativ SQLite Bazası:**
   Bütün xəstələr, həkimlər və loqlar layihənin kökündəki `clinic.db` faylında saxlanılır. İnternet tələb olunmur.

---

## 3. Bütün funksiyaların addım-addım test edilməsi

Sistemi yoxlamaq üçün bu ssenariləri ardıcıllıqla icra edin:

### Ssenari 1: Kəskin İnfarktın (STEMI) qeydiyyatı və Təcili Sirena
1. Yuxarı sağdakı **«⚡ STEMI İnfarkt»** şablon düyməsinə klikləyin.
2. Formada göstəricilərin avtomatik dolduğunu görün (Təzyiq: 205/110, SpO2: 89%, Şok indeksi hesablandı).
3. Sağ paneldə **«SƏVİYYƏ 1: QIRMIZI (Dərhal 0 dəqiqə)»** statusunun və diaqnozun `I21.9` yarandığını görün.
4. **«Qeydiyyata Al və Rəsmi Talon Yarat»** düyməsinə klikləyin:
   - Dərhal ikitonlu təcili tibbi sirena səslənəcək.
   - Bütün ekranın kənarında yanıb-sönən qırmızı həyəcan çərçivəsi aktivləşəcək.
   - Ekranda rəsmi ştrix-kodlu (Code 128) çap talonu açılacaq.
5. Həyəcan pəncərəsində **«Siqnalı Təsdiqlə və Dayandır (Acknowledge)»** düyməsinə basaraq səsi söndürün.

### Ssenari 2: Həkim Kabinetindən Növbəti Xəstənin Çağırılması
1. Yuxarı naviqasiya menyusundan **«Həkim Kabinetləri»** bölməsinə keçin.
2. İstənilən boş kabinetin altında **«Növbəti Pasiyenti Çağır»** düyməsinə klikləyin.
3. İki tonlu xəstəxana çağırış zəngi (Ding-Dong гонг) çalınacaq və yuxarıda çağırış bildirişi görünəcək.
4. Həmin xəstənin qəbulu bitdikdə **«Qəbulu Bitir»** düyməsini sıxaraq kabineti növbəti xəstə üçün azad edin.

### Ssenari 3: Gözləmə Zalı üçün Canlı Növbə Tablosu
1. Üst paneldəki **«Növbə Tablosu (Monitor)»** düyməsinə klikləyin.
2. Xəstəxana foyesi üçün nəzərdə tutulmuş böyük ekran açılacaq (Kim kabinetə çağırılıb, kim növbədə neçənci yerdədir).

### Ssenari 4: Rəsmi İstiqamətləndirmə Vərəqinin Çapı
1. **«Audit & Statistika»** tabına daxil olun.
2. Cədvəldə hər hansı xəstənin qarşısındakı **Printer (Çap)** ikonuna klikləyin.
3. Rəsmi Səhiyyə Nazirliyi başlıqlı, ştrix-kodlu, vital göstəricilər cədvəlli blank açılacaq. `Ctrl + P` basaraq kağıza çap edə bilərsiniz.

### Ssenari 5: Baş Həkim üçün Hesabatın CSV (Excel) İxracı
1. **«Audit & Statistika»** bölməsində **«CSV İxrac»** düyməsinə basın.
2. Bütün növbənin tibbi məlumatları kompüterinizə `.csv` faylı kimi yüklənəcək.

---

## 4. Fleşkart vasitəsilə istənilən kompüterdə 1-kliklə işə salma

Layihə 100% oflayn və portativdir.

### Fleşkartı hazırlamaq:
1. Bu layihə qovluğunu tam şəkildə (`node_modules`, `clinic.db`, `start.bat` daxil olmaqla) USB fleşkarta köçürün.

### İstənilən başqa noutbukda açmaq:
1. Fleşkartı həmin kompüterə taxın.
2. Həmin kompüterdə **Node.js** proqramının quraşdırıldığından əmin olun (əgər yoxdursa, rəsmi pulsuz [nodejs.org](https://nodejs.org) saytından LTS versiyanı 1 dəqiqəyə quraşdırın).
3. Fleşkartdakı **`start.bat`** faylına iki dəfə klikləyin.
4. Skript avtomatik olaraq:
   - Node.js mühitini yoxlayacaq;
   - `clinic.db` bazasını aşkar edəcək;
   - Daxili serveri başladacaq;
   - Avtomatik olaraq susmaya görə brauzeri `http://localhost:3000` ünvanında açacaq.

---

---

# 🇷🇺 ИНСТРУКЦИЯ НА РУССКОМ ЯЗЫКЕ

## 1. Что это за проект и зачем он нужен?

В отделениях скорой помощи и приёмных покоях (Emergency Department) решающее значение имеют первые минуты. Если пациент с начинающимся инфарктом или скрытым внутренним кровотечением будет сидеть в общей «живой» очереди позади людей с лёгким насморком, исход может быть летальным.

**MedTriage Pro** — это автоматизированное рабочее место (АРМ) медсестры триажа и дежурного врача-диспетчера. 
Оно реализует международный протокол **Manchester Triage System (MTS)**:
- Система мгновенно анализирует введённые жизненные показатели и жалобы.
- Вычисляет клинические индексы (включая **шоковый индекс Альговера**).
- Автоматически присваивает пациенту цветной уровень риска от 1 до 5 и предельное время ожидания.
- Назначает профильное отделение, дежурного врача и генерирует список неотложных анализов (STAT: ЭКГ, КТ, Тропонин и т.д.).
- При критических состояниях запускает звуковые медицинские сирены (Code Blue / Code Red) и оповещает персонал.

---

## 2. Как устроена логика и архитектура?

- **Frontend:** Next.js (App Router, React 19, TypeScript) с интерфейсом высокой информационной плотности в тёмной медицинской стилистике. Реализован пошаговый мастер (Step-by-Step), исключающий перегрузку медперсонала.
- **Backend & База данных:** SQLite (`better-sqlite3`) с хранилищем `clinic.db` прямо в корне проекта. Не требует установки MySQL/PostgreSQL или подключения к интернету.
- **Аудио-система:** Собственный генератор на базе браузерного **Web Audio API**. Сирены Code Blue (1000 Гц), Code Red (двухтональная 880/440 Гц), Code Yellow и больничный гонг вызова очереди (587 -> 880 Гц) генерируются программно и не требуют внешних аудиофайлов.
- **Печать:** Полный векторный генератор бланков направления с Code 128 штрих-кодом, таблицей витальных данных и местом для подписи врача.

---

## 3. Пошаговый сценарий тестирования всех функций

### Тест 1: Регистрация инфаркта миокарда (STEMI) и экстренная сирена
1. В верхней панели нажмите кнопку готового клинического пресета: **«⚡ STEMI İnfarkt»**.
2. Обратите внимание: автоматически подставились показатели: давление `205/110`, кислород `89%`, пульс `132`, шоковый индекс `0.64`.
3. В правой панели результат экспертной системы: **«SƏVİYYƏ 1: QIRMIZI (Dərhal 0 dəq)»**, подозрение на инфаркт `I21.9`.
4. Нажмите большую кнопку **«Qeydiyyata Al və Rəsmi Talon Yarat»**:
   - Включится двухтональная громкая сирена Code Red.
   - По периметру всего экрана начнёт пульсировать яркая красная рамка тревоги.
   - Откроется модальное окно экстренных оперативных действий.
5. В окне тревоги нажмите:
   - **«Təcili Yardımı Çağır»** — система отправит реальный запрос, переведёт реанимационный кабинет `REA-01` в статус «Təcili çağırış», запишет оперативный вызов бригады в базу данных и отобразит подтверждение доставки с точным временем.
   - **«Həkimə Bildiriş Göndər»** — система отправит срочный GSM/Pager сигнал дежурному кардиологу, залогирует факт отправки и выведет статус доставки.
   - **«Siqnalı Təsdiqlə və Dayandır (Acknowledge)»** — сирена выключится, факт подтверждения залогируется с точностью до секунды в журнале аудита.
6. Перейдите во вкладку **«Təcili Terminal»** или **«Həkim Kabinetləri»** — вы увидите живую запись вызова бригады и статус `REA-01: Təcili çağırış`!

### Тест 2: Вызов пациента в кабинет врача (Диспетчеризация)
1. Перейдите во вкладку **«Həkim Kabinetləri»** в верхней навигации.
2. Найдите любой свободный кабинет (со статусом «Boşdur») и нажмите **«Növbəti Pasiyenti Çağır»**.
3. Прозвучит двухтональный больничный гонг (Ding-Dong), вверху появится уведомление о вызове, а пациент закрепится за кабинетом со статусом «Xəstə qəbulunda».
4. По завершении приёма нажмите **«Qəbulu Bitir»**.

### Тест 3: Экран электронной очереди для зала ожидания
1. В шапке нажмите кнопку **«Növbə Tablosu (Monitor)»**.
2. Откроется полноэкранное информационное табло реального времени: кто вызван в кабинет прямо сейчас и кто ожидает в очереди по уровням срочности MTS.

### Тест 4: Печать официального талона со штрихкодом
1. Перейдите во вкладку **«Audit & Statistika»**.
2. В таблице зарегистрированных пациентов нажмите иконку **Принтера**.
3. Откроется адаптивный медицинский талон со штрих-кодом, таблицей давления, сатурации, аллергиями и назначенными анализами. Можно распечатать через стандартный диалог печати (`Ctrl + P`).

### Тест 5: Выгрузка отчёта за смену в Excel/CSV
1. Во вкладке **«Audit & Statistika»** нажмите кнопку **«CSV İxrac»**.
2. Браузер моментально скачает файл `.csv`, открывающийся в Microsoft Excel с полной историей всех обращений.

---

## 4. Как запустить проект с флешки на любом ноутбуке

Проект полностью автономен и не привязан к конкретному компьютеру.

### Шаг 1. Подготовка флешки
Скопируйте всю папку проекта на USB-флешку. На флешке обязательно должны присутствовать:
- Папка `node_modules`
- Файл базы данных `clinic.db`
- Скрипт запуска `start.bat`
- Файлы проекта (`package.json`, папка `app`, `components`, `lib`)

### Шаг 2. Запуск на стороннем ноутбуке
1. Вставьте флешку в целевой ноутбук (Windows).
2. Убедитесь, что на ноутбуке установлен **Node.js** (версия 18, 20 или 22+). Если его нет, скачайте официальный установщик с [nodejs.org](https://nodejs.org) — это занимает меньше минуты.
3. Откройте папку на флешке и **дважды кликните по файлу `start.bat`**.
4. Скрипт `start.bat` автоматически:
   - Проверит наличие Node.js в системе;
   - Обнаружит базу данных `clinic.db` прямо на флешке;
   - Запустит сервер `Next.js`;
   - Через 3 секунды сам откроет браузер по адресу `http://localhost:3000`.
5. Все внесённые изменения (новые пациенты, смены статусов врачей, журнал тревог) будут сохраняться прямо в файл `clinic.db` на вашей флешке.
