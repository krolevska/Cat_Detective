// =====================================================
// QUEST: Tutorial
// SCENE: Train_Station
// FILE: Whisker_Noir_Prologue_Refactored.ink
// =====================================================

// =====================================================
// UNITY TRIGGER REGISTRY
// =====================================================
// scene_start                         -> prologue_start
//
// proximity:newspaper_vendor_area     -> ambient_newspaper_vendor
// interact:newspaper_vendor           -> newspaper_vendor
//
// proximity:homeless_cat_area         -> ambient_homeless_cat
// interact:homeless_cat               -> homeless_cat
//
// proximity:taxi_area                 -> pass_taxi_thought
// interact:taxi_driver                -> taxi_driver
//
// interact:station_sign               -> inspect_station_sign
// interact:newspaper_stand            -> inspect_newspaper_stand
// interact:street_notice_board        -> street_notice_board
//
// proximity:apartment_entrance_area  -> near_apartment_thought
// interact:apartment_door             -> apartment_entrance


// =====================================================
// GLOBAL META TAGS
// =====================================================

# quest_id: prologue_station
# quest_title: Пролог — Дорога додому
# scene_id: station_prologue


// =====================================================
// EXTERNAL FUNCTIONS
// Ink -> Unity game events
// =====================================================

EXTERNAL AddFact(factId)
EXTERNAL UnlockConclusion(conclusionId)
EXTERNAL UpdateObjective(objectiveId)
EXTERNAL CompleteObjective(objectiveId)


// =====================================================
// GLOBAL QUEST STATE
// =====================================================

VAR prologue_started = false
VAR prologue_completed = false

VAR objective_reach_apartment_started = false
VAR objective_reach_apartment_completed = false

VAR route_taxi = false
VAR route_walk = false

VAR reached_apartment_building = false
VAR tutorial_conclusion_completed = false

// Який саме висновок гравець обрав:
// 0 — ще не обрано
// 1 — "У тексті просто помилка"
// 2 — "Хтось навмисно переписує історію"
// 3 — "Мої спогади можуть бути менш надійними"
VAR orphanage_conclusion = 0


// =====================================================
// KNOWLEDGE FLAGS
// Що Томмі вже дізнався або помітив
// =====================================================

VAR heard_orphanage_news = false
VAR noticed_orphanage_inconsistency = false
VAR heard_missing_people_rumor = false

VAR fact_orphanage_inconsistency_added = false
VAR conclusion_orphanage_unlocked = false

VAR fact_missing_people_memory_loss_added = false
VAR fact_erased_names_added = false


// =====================================================
// INTERACTION FLAGS
// Що гравець уже робив, щоб не дублювати інформацію
// =====================================================

VAR read_station_sign = false
VAR inspected_newspaper = false
VAR inspected_notice_board = false
VAR saw_missing_poster = false

VAR spoke_to_vendor = false
VAR vendor_gave_directions = false
VAR vendor_discussed_orphanage_interest = false

VAR spoke_to_homeless = false
VAR asked_homeless_for_help = false
VAR asked_homeless_for_useful_info = false
VAR told_homeless_to_move = false
VAR homeless_missing_topic_unlocked = false
VAR homeless_explained_missing_people = false
VAR asked_homeless_about_loss_without_trust = false
VAR homeless_opened_up = false
VAR gave_coin_to_homeless = false
VAR dismissed_homeless_rumor = false

VAR spoke_to_taxi_driver = false
VAR driver_gave_city_update = false
VAR asked_driver_about_disappearances = false
VAR asked_driver_about_city_changes = false

VAR saw_vendor_ambient = false
VAR saw_homeless_ambient = false
VAR saw_taxi_thought = false
VAR saw_apartment_thought = false


// =====================================================
// SOFT BEHAVIOR TRACKING
// Пізніше можна використати в репліках або прихованих перевірках
// =====================================================

VAR justice_points = 0
VAR pragmatic_points = 0
VAR aggression_points = 0


// =====================================================
// STYLE DICTIONARY
// =====================================================
// style: narration             | звичайний опис / сценічний текст
// style: protagonist           | репліка Томмі в діалозі
// style: npc                   | репліка NPC
// style: inner_thought         | внутрішня думка Томмі
// style: document_text         | газетні заголовки, таблички, листи
// style: system_fact           | повідомлення про новий факт
// style: system_quest          | повідомлення про нове / оновлене завдання
// style: system_quest_complete | повідомлення про виконаний квест
// style: system_conclusion     | повідомлення про відкритий / зафіксований висновок


// =====================================================
// UI DICTIONARY
// =====================================================
// ui: dialogue_panel       | велика панель діалогу й choices
// ui: thought_bottom       | коротка думка унизу екрана
// ui: thought_above_player | коротка думка над Томмі
// ui: toast                | системне повідомлення: факт, квест, висновок
// ui: ambient_subtitle     | короткий атмосферний текст без паузи гри
// ui: document_view        | газета, лист, табличка, оголошення


// =====================================================
// AUTHORING TEMPLATE
// =====================================================
/* ---------------------------------
=== knot_name ===
# trigger: interaction / proximity / scene_start
# ui: dialogue_panel / thought_bottom / thought_above_player / ambient_subtitle / document_view
# prompt: Текст підказки біля предмета
# repeatable: true / false

# speaker: internal_id
# name: Ім'я в UI
# avatar: avatar_id
# style: narration / npc / inner_thought / ...
Текст.
----------------------------------*/


// =====================================================
// SCENE START
// =====================================================

=== prologue_start ===
# trigger: scene_start
# ui: dialogue_panel
# repeatable: false

{ prologue_started == false:
    ~ prologue_started = true

    # style: narration
    Дощ зустрів тебе ще до того, як місто встигло це зробити.

    # style: narration
    Він стікав із даху вокзалу довгими холодними нитками, розбивався об мокрий камінь перону й тихо шипів у калюжах біля рейок.

    # style: narration
    Потяг, який привіз тебе назад, уже готувався рушати далі. Наче йому не терпілося залишити це місце.

    # style: narration
    У лапі — потерта валіза. У кишені — ключ від квартири, де ніхто не чекає.

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Квартира. Тепле світло. Можливо, вершки. Можливо, тиша.

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Цілком пристойний план на вечір.

    { objective_reach_apartment_started == false:
        ~ UpdateObjective("reach_apartment")
        ~ objective_reach_apartment_started = true

        # ui: toast
        # style: system_quest
        [ЗАВДАННЯ ОНОВЛЕНО: Дістатися квартири.]
    }
}

-> DONE


// =====================================================
// PASSIVE / PROXIMITY TEXTS
// Короткі атмосферні рядки, які не мають відкривати діалогову панель
// =====================================================

=== ambient_newspaper_vendor ===
# trigger: proximity
# ui: ambient_subtitle
# repeatable: false

{ saw_vendor_ambient == false:
    # style: narration
    Під навісом газетного кіоску тісняться мокрі шпальти, лотерейні квитки й продавець із поглядом людини, яка вже нічому не дивується.

    ~ saw_vendor_ambient = true
}

-> DONE


=== ambient_homeless_cat ===
# trigger: proximity
# ui: ambient_subtitle
# repeatable: false

{ saw_homeless_ambient == false:
    # style: narration
    Під вокзальним навісом сидить безхатько в старому пальті. Біля лап тьмяно блищить бляшана кружка.

    ~ saw_homeless_ambient = true
}

-> DONE


=== pass_taxi_thought ===
# trigger: proximity
# ui: thought_bottom
# repeatable: false

{ saw_taxi_thought == false:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Чорне таксі під жовтим ліхтарем. Сухо, швидко й трохи менш чесно, ніж іти пішки.

    ~ saw_taxi_thought = true
}

-> DONE


=== near_apartment_thought ===
# trigger: proximity
# ui: thought_bottom
# repeatable: false

{ saw_apartment_thought == false:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Дім. Формально.

    ~ saw_apartment_thought = true
}

-> DONE


// =====================================================
// NPC DIALOGUES
// У цих knots — мінімум оточення, максимум власне розмови.
// Опис локацій винесено в proximity knots вище.
// =====================================================

=== newspaper_vendor ===
# trigger: interaction
# ui: dialogue_panel
# prompt: Поговорити з газетярем
# repeatable: true

{ spoke_to_vendor:
    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Ще один погляд на катастрофи дня?
- else:
    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Вечірній випуск! Мерія економить, дощ тримається, решта теж не обнадіює!

    ~ spoke_to_vendor = true
}

-> newspaper_vendor_menu


=== newspaper_vendor_menu ===

+ { heard_orphanage_news == false } [Запитати про заголовок на першій шпальті.]
    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Старий міський сиротинець остаточно закрили. Мерія каже — будівля давно стояла без діла, тягнула гроші з бюджету.

    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Гарна формула, до речі. Спершу чогось не доглядають роками, потім називають це марнотратством.

    ~ heard_orphanage_news = true

    -> newspaper_vendor_menu


+ { inspected_newspaper == false } [Попросити дати глянути газету.]
    -> inspect_newspaper_from_vendor


+ { vendor_gave_directions == false } [Запитати, як пройти до житлових кварталів.]
    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Через центральний вихід. Побачиш трамвайні рейки — йди вздовж них.

    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Якщо потрібен комфорт, таксисти праворуч. Якщо характер — прямо.

    ~ vendor_gave_directions = true

    -> newspaper_vendor_menu


+ { heard_orphanage_news == true && vendor_discussed_orphanage_interest == false } [Запитати, чому всіх так зацікавив сиротинець.]
    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Бо людям подобається сумувати про будівлі більше, ніж про мешканців. Будівлі не просять пояснень.

    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Хтось каже, там зроблять архів. Хтось — склад. Хтось — що його просто знесуть і поставлять чергову коробку з чиновниками.

    ~ vendor_discussed_orphanage_interest = true

    -> newspaper_vendor_menu


+ [Закінчити розмову.]
    # speaker: npc_newspaper_vendor
    # name: Газетяр
    # avatar: newspaper_vendor_01
    # style: npc
    Не читай усе підряд. Місто й без того важке для травлення.

    -> DONE


=== homeless_cat ===
# trigger: interaction
# ui: dialogue_panel
# prompt: Поговорити з безхатьком
# repeatable: true

{ spoke_to_homeless:
    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Знову ти. Питання не закінчилися?
- else:
    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Повернувся, детективе? Чи просто ще не встиг утекти?

    ~ spoke_to_homeless = true
}

-> homeless_first_menu


=== homeless_first_menu ===

+ { asked_homeless_for_help == false } [Запитати, чи йому потрібна допомога.]
    ~ asked_homeless_for_help = true
    ~ justice_points = justice_points + 1
    ~ homeless_missing_topic_unlocked = true

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Допомога? Мені?

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Мені потрібні сухі шкарпетки, гаряча юшка й місто, яке перестане ковтати своїх мешканців. Обирай, із чим почнеш.

    -> homeless_open_menu


+ { asked_homeless_for_useful_info == false } [Запитати, чи він бачив тут щось корисне.]
    ~ asked_homeless_for_useful_info = true
    ~ pragmatic_points = pragmatic_points + 1
    ~ homeless_missing_topic_unlocked = true

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Корисне для кого? Для правди? Для поліції? Для того, хто платить?

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Але так. Бачив. Завжди щось бачу. Саме тому мені й не дуже добре спиться.

    -> homeless_open_menu


+ { told_homeless_to_move == false } [Сказати, щоб він не перегороджував прохід.]
    ~ told_homeless_to_move = true
    ~ aggression_points = aggression_points + 1
    ~ homeless_missing_topic_unlocked = true

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Так, звісно. Я і є головна перешкода на шляху цивілізації.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Дивися, щоб і тебе не прибрали з дороги.

    -> DONE


+ [Піти.]
    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Місто любить тих, хто не ставить зайвих питань. Недовго, але любить.

    -> DONE


=== homeless_open_menu ===

+ { homeless_missing_topic_unlocked == true && homeless_explained_missing_people == false } [Запитати, що він мав на увазі про місто, яке “ковтає” мешканців.]
    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Люди зникають.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Не “поїхали до родичів”. Не “забули сплатити оренду й втекли”. Зникають.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Учора кіт спить за два метри від мене під газетами. Я знаю, як його звати. Знаю, що він хропе, коли мерзне.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Наступного ранку газети лишаються. Кіт — ні. І ніхто не може згадати, про кого я питаю.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Таке місто. Викидає зайве. А потім робить вигляд, що сміття тут ніколи не було.

    ~ heard_missing_people_rumor = true
    ~ homeless_explained_missing_people = true

    { fact_missing_people_memory_loss_added == false:
        ~ AddFact("people_disappear_and_are_forgotten")
        ~ fact_missing_people_memory_loss_added = true

        # ui: toast
        # style: system_fact
        [НОВИЙ ФАКТ ОТРИМАНО: Безхатько стверджує, що в місті люди не просто зникають — їх перестають пам’ятати.]
    }

    -> homeless_open_menu


+ { homeless_explained_missing_people == true && homeless_opened_up == false && (gave_coin_to_homeless || justice_points > 0) } [Запитати, чи він сам когось втратив.]
    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Була одна кицька. Руда. Завжди співала собі під ніс, коли рилася в контейнерах за булочками.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Я пам’ятаю голос. Пам’ятаю, як вона сміялася з моїх жартів, хоча жарти були погані.

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    А от ім’я ніби хтось вирізав ножем.

    ~ homeless_opened_up = true

    { fact_erased_names_added == false:
        ~ AddFact("names_can_disappear_from_memory")
        ~ fact_erased_names_added = true

        # ui: toast
        # style: system_fact
        [НОВИЙ ФАКТ ОТРИМАНО: Після зникнення людини можуть зникати навіть спогади про її ім’я.]
    }

    -> homeless_open_menu


+ { homeless_explained_missing_people == true && homeless_opened_up == false && !(gave_coin_to_homeless || justice_points > 0) && asked_homeless_about_loss_without_trust == false } [Запитати, чи він сам когось втратив.]
    ~ asked_homeless_about_loss_without_trust = true

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Усі когось втрачали. Просто не всі достатньо чесні, щоб це визнавати.

    -> homeless_open_menu


+ { gave_coin_to_homeless == false } [Кинути монету в кружку.]
    ~ gave_coin_to_homeless = true
    ~ justice_points = justice_points + 1

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Дякую. Не за монету. За те, що не вдав, ніби мене тут немає.

    -> homeless_open_menu


+ { heard_missing_people_rumor == true && dismissed_homeless_rumor == false } [Сказати, що звучить як п’яна маячня.]
    ~ dismissed_homeless_rumor = true
    ~ aggression_points = aggression_points + 1

    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Авжеж. А коли те саме скаже хтось у краватці, ти назвеш це “свідченням”.

    -> homeless_open_menu


+ [Закінчити розмову.]
    # speaker: npc_homeless
    # name: Безхатько
    # avatar: homeless_01
    # style: npc
    Якщо сьогодні когось шукатимеш — записуй ім’я. Завтра воно може здатися вигадкою.

    -> DONE


=== taxi_driver ===
# trigger: interaction
# ui: dialogue_panel
# prompt: Поговорити з таксистом
# repeatable: true

{ spoke_to_taxi_driver:
    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Вирішив, куди тебе везти? Чи дощ ще не достатньо переконливий?
- else:
    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Потрібна машина? Чи ти з тих героїв, що воліють знайомитися з містом через промоклі черевики?

    ~ spoke_to_taxi_driver = true
}

-> taxi_menu


=== taxi_menu ===

+ [Назвати адресу квартири й сісти в таксі.]
    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Сідай. Місто виглядає менш привітним із тротуару.

    -> taxi_ride


+ { heard_orphanage_news == false && driver_gave_city_update == false } [Запитати, що нового в місті.]
    ~ driver_gave_city_update = true
    ~ heard_orphanage_news = true

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Нового? Мерія закрила старий сиротинець. Каже, давно не потрібен.

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    У нас тут усе стає “непотрібним” рівно в той момент, коли за нього більше не хочеться відповідати.

    -> taxi_menu


+ { heard_missing_people_rumor == true && asked_driver_about_disappearances == false } [Запитати, чи правда, що в місті зникають люди.]
    ~ asked_driver_about_disappearances = true

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Люди завжди зникають. Через борги, через кохання, через дурість. Іноді через усе одразу.

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Але останнім часом... Так. Чутки є. Наче деякі квартири спорожніли так чисто, що навіть сусіди не певні, чи хтось там жив.

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Я не люблю такі розмови під час роботи. Погано впливають на чайові.

    -> taxi_menu


+ { asked_driver_about_city_changes == false } [Запитати, чи місто сильно змінилося.]
    ~ asked_driver_about_city_changes = true

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Деякі речі стали гіршими. Інші — дорожчими. Ще інші просто зникли, і тепер усі вдають, що так і було.

    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Бар на розі, наприклад. Колись його не було. А може, був. З віком пам’ять працює як міська рада.

    -> taxi_menu


+ [Сказати, що підеш пішки.]
    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Як знаєш. Якщо передумаєш, шукай жовті ліхтарі й погані рішення.

    -> walk_departure


+ [Відійти.]
    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    У всіх сьогодні свій маршрут.

    -> DONE


// =====================================================
// INTERACTIONS WITH OBJECTS
// Огляди об’єктів — не як діалог, а як document / examine content.
// =====================================================

=== inspect_station_sign ===
# trigger: interaction
# ui: document_view
# prompt: Оглянути табличку
# repeatable: true

{ read_station_sign:
    # style: document_text
    СТОЯНКА ТАКСІ — праворуч.  
    ЦЕНТРАЛЬНА ПЛОЩА — прямо.  
    ЖИТЛОВІ КВАРТАЛИ — через площу, вниз уздовж трамвайних колій.

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Якщо заблукаєш тут у перші п’ять хвилин після повернення, доведеться списати це на дощ.
- else:
    # style: document_text
    СТОЯНКА ТАКСІ — праворуч.  
    ЦЕНТРАЛЬНА ПЛОЩА — прямо.  
    ЖИТЛОВІ КВАРТАЛИ — через площу, вниз уздовж трамвайних колій.

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Місто ніби не дуже хотіло, щоб тут губилися. Принаймні — не випадково.

    ~ read_station_sign = true
}

-> DONE


=== inspect_newspaper_from_vendor ===
# ui: document_view

{ inspected_newspaper:
    # style: document_text
    Ти вже бачив головну шпальту. Та сама стаття. Та сама цифра.
- else:
    # style: document_text
    “МЕРІЯ ЗАКРИВАЄ СТАРИЙ СИРОТИНЕЦЬ: БУДІВЛЯ НЕ ВИКОРИСТОВУВАЛАСЯ ЗА ПРИЗНАЧЕННЯМ ПОНАД П’ЯТНАДЦЯТЬ РОКІВ”.

    ~ inspected_newspaper = true
    ~ heard_orphanage_news = true

    { noticed_orphanage_inconsistency == false:
        # speaker: tommy_inner
        # name: Томмі
        # avatar: none
        # style: inner_thought
        П’ятнадцять років? Ні. Ти покинув той сиротинець значно пізніше.

        ~ noticed_orphanage_inconsistency = true

        { fact_orphanage_inconsistency_added == false:
            ~ AddFact("official_info_about_the_orphanage_doesnt_fit_tommis_memory")
            ~ fact_orphanage_inconsistency_added = true

            # ui: toast
            # style: system_fact
            [НОВИЙ ФАКТ ОТРИМАНО: Офіційна версія про сиротинець не збігається зі спогадами Томмі.]
        }

        { conclusion_orphanage_unlocked == false:
            ~ UnlockConclusion("orphanage")
            ~ conclusion_orphanage_unlocked = true

            # ui: toast
            # style: system_conclusion
            [НОВИЙ ВИСНОВОК ДОСТУПНИЙ: “Сиротинець”.]
        }
    - else:
        # speaker: tommy_inner
        # name: Томмі
        # avatar: none
        # style: inner_thought
        Ти вже знаєш, що ця офіційна версія не сходиться з твоєю пам’яттю.
    }
}

-> newspaper_vendor_menu


=== inspect_newspaper_stand ===
# trigger: interaction
# ui: document_view
# prompt: Оглянути газети
# repeatable: true

{ inspected_newspaper:
    # style: document_text
    Ти вже бачив головну шпальту. Та сама стаття про сиротинець.
- else:
    # style: document_text
    “МЕРІЯ ЗАКРИВАЄ СТАРИЙ СИРОТИНЕЦЬ.”

    # style: document_text
    “Будівля не використовувалась за призначенням понад п’ятнадцять років.”

    ~ inspected_newspaper = true
    ~ heard_orphanage_news = true

    { noticed_orphanage_inconsistency == false:
        # speaker: tommy_inner
        # name: Томмі
        # avatar: none
        # style: inner_thought
        Понад п’ятнадцять? Ні. Ти жив там пізніше.

        ~ noticed_orphanage_inconsistency = true

        { fact_orphanage_inconsistency_added == false:
            ~ AddFact("official_info_about_the_orphanage_doesnt_fit_tommis_memory")
            ~ fact_orphanage_inconsistency_added = true

            # ui: toast
            # style: system_fact
            [НОВИЙ ФАКТ ОТРИМАНО: Офіційна версія про сиротинець не збігається зі спогадами Томмі.]
        }

        { conclusion_orphanage_unlocked == false:
            ~ UnlockConclusion("orphanage")
            ~ conclusion_orphanage_unlocked = true

            # ui: toast
            # style: system_conclusion
            [НОВИЙ ВИСНОВОК ДОСТУПНИЙ: “Сиротинець”.]
        }
    - else:
        # speaker: tommy_inner
        # name: Томмі
        # avatar: none
        # style: inner_thought
        Знайома формула. Ти вже впіймав її на брехні.
    }
}

-> DONE


=== street_notice_board ===
# trigger: interaction
# ui: document_view
# prompt: Оглянути дошку оголошень
# repeatable: true

{ inspected_notice_board:
    # style: document_text
    Повідомлення мерії про сиротинець — зверху. Під ним усе ще висить розмитий плакат про зниклого мешканця.

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Місто любить шари паперу. Так легше не бачити, що було під ними.
- else:
    # style: document_text
    “МЕРІЯ ІНФОРМУЄ: СТАРИЙ СИРОТИНЕЦЬ ОСТАТОЧНО ВИВЕДЕНО З ЕКСПЛУАТАЦІЇ.”

    # style: document_text
    “Будівля не використовувалась за призначенням понад п’ятнадцять років.”

    ~ heard_orphanage_news = true

    { noticed_orphanage_inconsistency == false:
        # speaker: tommy_inner
        # name: Томмі
        # avatar: none
        # style: inner_thought
        Знову ця цифра. Понад п’ятнадцять років — і знову нісенітниця.

        ~ noticed_orphanage_inconsistency = true

        { fact_orphanage_inconsistency_added == false:
            ~ AddFact("official_info_about_the_orphanage_doesnt_fit_tommis_memory")
            ~ fact_orphanage_inconsistency_added = true

            # ui: toast
            # style: system_fact
            [НОВИЙ ФАКТ ОТРИМАНО: Повідомлення мерії суперечить пам’яті Томмі про сиротинець.]
        }

        { conclusion_orphanage_unlocked == false:
            ~ UnlockConclusion("orphanage")
            ~ conclusion_orphanage_unlocked = true

            # ui: toast
            # style: system_conclusion
            [НОВИЙ ВИСНОВОК ДОСТУПНИЙ: “Сиротинець”.]
        }
    - else:
        # speaker: tommy_inner
        # name: Томмі
        # avatar: none
        # style: inner_thought
        Та сама офіційна версія. Ти вже знаєш, де в ній тріщина.
    }

    # style: document_text
    Нижче, наполовину залите дощем, висить інше оголошення: “ЗНИК…”

    # style: document_text
    Ім’я розмилося або було відірване. Лишилася тільки розмита фотографія сірого кота в пальті й номер, де останні цифри зникли під чужою рекламою.

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Ти не знаєш, чи цей кіт знайшовся. Дошка не відповідає.

    ~ saw_missing_poster = true
    ~ inspected_notice_board = true
}

-> DONE


// =====================================================
// ROUTE BEATS
// =====================================================

=== taxi_ride ===
# ui: dialogue_panel

~ route_taxi = true
~ route_walk = false

# style: narration
Ти зачиняєш дверцята, і дощ одразу стає чужою проблемою.

# style: narration
Таксі плавно відходить від вокзалу. За вікном місто тягнеться вгору мокрими фасадами, неоновими вивісками й темними вікнами.

{ heard_orphanage_news == false:
    # speaker: npc_taxi_driver
    # name: Таксист
    # avatar: taxi_driver_01
    # style: npc
    Бачив? Сиротинець закривають. Кажуть, давно стояв порожній.

    ~ heard_orphanage_news = true
}

# style: document_text
На муніципальному плакаті за вікном написано: “СТАРИЙ СИРОТИНЕЦЬ ЛІКВІДОВАНО. БУДІВЛЯ НЕ ВИКОРИСТОВУЄТЬСЯ ЗА ПРИЗНАЧЕННЯМ ПОНАД П’ЯТНАДЦЯТЬ РОКІВ.”

{ noticed_orphanage_inconsistency == false:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    П’ятнадцять років тому ти ще був там. Якщо будівля справді стояла порожньою, хто тоді відкривав тобі двері щоранку?

    ~ noticed_orphanage_inconsistency = true

    { fact_orphanage_inconsistency_added == false:
        ~ AddFact("official_info_about_the_orphanage_doesnt_fit_tommis_memory")
        ~ fact_orphanage_inconsistency_added = true

        # ui: toast
        # style: system_fact
        [НОВИЙ ФАКТ ОТРИМАНО: Офіційна інформація про сиротинець суперечить спогадам Томмі.]
    }

    { conclusion_orphanage_unlocked == false:
        ~ UnlockConclusion("orphanage")
        ~ conclusion_orphanage_unlocked = true

        # ui: toast
        # style: system_conclusion
        [НОВИЙ ВИСНОВОК ДОСТУПНИЙ: “Сиротинець”.]
    }
- else:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Та сама офіційна цифра миготить уже й на міських плакатах.
}

# speaker: npc_taxi_driver
# name: Таксист
# avatar: taxi_driver_01
# style: npc
Приїхали. Сподіваюся, вдома в тебе сухіше, ніж у світі.

~ reached_apartment_building = true

-> apartment_entrance


=== walk_departure ===
# ui: dialogue_panel

~ route_walk = true
~ route_taxi = false

# style: narration
Ти обираєш пішу дорогу.

# style: narration
Повітря пахне мокрим каменем, димом і жирною їжею з кіоску. Над головою скрегоче трамвайна лінія.

# style: document_text
На розі висить офіційний плакат мерії: “СТАРИЙ СИРОТИНЕЦЬ ЛІКВІДОВАНО. БУДІВЛЯ НЕ ВИКОРИСТОВУЄТЬСЯ ЗА ПРИЗНАЧЕННЯМ ПОНАД П’ЯТНАДЦЯТЬ РОКІВ.”

{ heard_orphanage_news == false:
    ~ heard_orphanage_news = true
}

{ noticed_orphanage_inconsistency == false:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Понад п’ятнадцять років? Ні. Ти жив там пізніше. Ти пам’ятаєш його занадто добре, щоб погодитися з друкованою брехнею.

    ~ noticed_orphanage_inconsistency = true

    { fact_orphanage_inconsistency_added == false:
        ~ AddFact("official_info_about_the_orphanage_doesnt_fit_tommis_memory")
        ~ fact_orphanage_inconsistency_added = true

        # ui: toast
        # style: system_fact
        [НОВИЙ ФАКТ ОТРИМАНО: Офіційна інформація про сиротинець суперечить спогадам Томмі.]
    }

    { conclusion_orphanage_unlocked == false:
        ~ UnlockConclusion("orphanage")
        ~ conclusion_orphanage_unlocked = true

        # ui: toast
        # style: system_conclusion
        [НОВИЙ ВИСНОВОК ДОСТУПНИЙ: “Сиротинець”.]
    }
- else:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Ще один плакат. Та сама версія, яку ти вже не приймаєш на віру.
}

# style: narration
За кілька кварталів звідси — твій будинок. Можна прямувати далі.

-> DONE


// =====================================================
// APARTMENT / CONCLUSION FLOW
// =====================================================

=== apartment_entrance ===
# trigger: interaction
# ui: dialogue_panel
# prompt: Підійти до дверей
# repeatable: true

~ reached_apartment_building = true

{ tutorial_conclusion_completed:
    -> apartment_final_text
- else:
    # style: narration
    Ти вже дістаєш ключ, але думка про сиротинець не відпускає.

    # style: narration
    Офіційна версія й твоя пам’ять стоять поруч. Їх неможливо не звести докупи.

    -> tutorial_first_conclusion
}


=== tutorial_first_conclusion ===

{ heard_orphanage_news && noticed_orphanage_inconsistency && conclusion_orphanage_unlocked:
    -> tutorial_first_conclusion_intro
- else:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Поки що думки не складаються в ясний висновок. Ти відчуваєш, що пропустив якусь важливу деталь про сиротинець.

    -> DONE
}


=== tutorial_first_conclusion_intro ===
# ui: dialogue_panel

# ui: toast
# style: system_conclusion
[ТУТОРІАЛ: ВИСНОВКИ]

# style: narration
Іноді окремі факти нічого не означають. Але варто поставити їх поруч — і між ними з’являється тріщина.

# style: narration
ФАКТ 1: Мерія стверджує, що старий сиротинець не використовувався за призначенням понад п’ятнадцять років.

# style: narration
ФАКТ 2: Томмі особисто жив у цьому сиротинці значно пізніше.

# style: narration
Який попередній висновок ти робиш?

+ [У тексті просто помилка. Буває.]
    ~ orphanage_conclusion = 1

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Можливо. Газети помиляються. Чиновники помиляються. І все ж ця конкретна помилка неприємно дряпає зсередини.

    -> tutorial_first_conclusion_finish


+ [Хтось навмисно переписує історію сиротинцю.]
    ~ orphanage_conclusion = 2

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Дивна брехня. Маленька, акуратна, майже непомітна. Саме такі брехні й варто запам’ятовувати.

    -> tutorial_first_conclusion_finish


+ [Мої спогади можуть бути менш надійними, ніж мені хотілося б.]
    ~ orphanage_conclusion = 3

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Неприємна, але чесна думка. Пам’ять не зберігає минуле — вона щодня відтворює його з уламків. Та сиротинець — не розмитий сон.

    -> tutorial_first_conclusion_finish


=== tutorial_first_conclusion_finish ===

{ tutorial_conclusion_completed == false:
    ~ tutorial_conclusion_completed = true

    # ui: toast
    # style: system_conclusion
    [ВИСНОВОК ЗАФІКСОВАНО: Офіційна історія сиротинцю не збігається зі спогадами Томмі.]
}

-> apartment_final_text


=== apartment_final_text ===

{ prologue_completed:
    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Лист Кітті вже у твоїй лапі. Час прочитати його.

    -> DONE
- else:
    # style: narration
    Ключ нарешті входить у замок.

    # style: narration
    На килимку біля дверей лежить конверт. Без марки. Лише твоє ім’я.

    # style: document_text
    “Томмі. Повернешся — знайди мене. Кітті.”

    # speaker: tommy_inner
    # name: Томмі
    # avatar: none
    # style: inner_thought
    Знайомий витончений почерк. Ти впізнав би його навіть крізь дощ.

    ~ prologue_completed = true

    { objective_reach_apartment_completed == false:
        ~ CompleteObjective("reach_apartment")
        ~ objective_reach_apartment_completed = true

        # ui: toast
        # style: system_quest_complete
        [ЗАВДАННЯ ВИКОНАНО: Дістатися квартири.]
    }

    # ui: toast
    # style: system_quest
    [НОВЕ ЗАВДАННЯ МОЖЕ БУТИ ДОДАНО ПІЗНІШЕ: Прочитати записку Кітті.]

    -> DONE
}
