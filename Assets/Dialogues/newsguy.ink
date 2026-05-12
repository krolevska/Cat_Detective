VAR money = 50
VAR goodReputation = 0
VAR detectiveReputation = 0
VAR cigarettes = 10
VAR spentMoney = 0
VAR acquiredCigarettes = 0
VAR add_new_info_quest_01 = false
VAR add_new_info_quest_02 = false

-> Start_of_talk_with_newsguy

=== Start_of_talk_with_newsguy ===
Газетяр: Газета свіжа. Читати будеш? Сім гривень штука.

* [Давай]
    ~ spentMoney = 7
    -> Bought_the_newspaper

* [Давай, решту залиш собі]
    ~ spentMoney = 10
    ~ goodReputation = goodReputation + 1
    -> Bought_the_newspaper

+ [Я тільки за цигарками]
    ~ spentMoney = 4
    ~ acquiredCigarettes = 20
    -> Start_of_talk_with_newsguy

* [Що нового у місті?]
    -> Neutral_ending

* [Піти]
    -> END


=== Bought_the_newspaper ===
Газетяр: Може ще чого?

+ [Я тільки за цигарками]
    ~ spentMoney = spentMoney + 4
    ~ acquiredCigarettes = 20
    -> Bought_the_newspaper

* [Що нового у місті?]
    { goodReputation >= 1:
        -> Sympaty_high
    - else:
        -> Neutral_ending
    }

* [Піти]
    -> END


=== Sympaty_high ===
Газетяр: Розповідає останні новини

* [Розкажи більше про Х.]
    ~ detectiveReputation = detectiveReputation + 1
    -> X_story

* [Розкажи більше про Y.]
    ~ detectiveReputation = detectiveReputation + 1
    -> Y_story

* [Мені вже пора йти. Можливо, ще повернуся.]
    -> END


=== X_story ===
Газетяр: Розповідає детальніше про X

* [Повернутися]
    ~ add_new_info_quest_01 = true
    -> Sympaty_high


=== Y_story ===
Газетяр: Розповідає детальніше про Y

* [Повернутися]
    ~ add_new_info_quest_02 = true
    -> Sympaty_high


=== Neutral_ending ===
Газетяр: Мені більше нема чого тобі сказати.
-> END
