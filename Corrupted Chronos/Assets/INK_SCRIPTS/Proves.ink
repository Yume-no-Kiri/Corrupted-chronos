VAR next_scene = "Main_menu"



=== npc1 === //node
//si encara no ha acceptat la missio
HI  //text tal qual
I will give you a very important quest.
Do you accept?
* [YES]
    Nice!
* [NO]
    -> npc1
//Si ja ha acceptat la misió
What are you waiting for?

//completada
Perfect you have done it!
    
    

- -> END //final diàleg

=== npc2 ===
    //si no has iniciat la quest
    ...
    //si has iniciat la quest
    Nice you completed the quest!

- -> END

=== garatge ===
    //si no has iniciat la quest
    ...
    //si has iniciat la quest
   Hola... Aquí pots equipar diferents armes a la nau.
    Pel moment tens disponible la metralleta i l'escopeta.
    La característica principal de la metralleta és el seu tir ràpid.
    Per activar-la fes clic esquerre.
    L'escopeta és una arma que té múltiples dispars però de poc abast.
    Per activar-la clic dret

- -> END
=== npc_pres===
    Benvingut a Corrupted chronos!
    La teva missió serà la d'alliberar la galàxia.
    De moment ves al garatge que es troba aquí a sota.
    Ell et dirà quines opcions tens.

- -> END
=== tir ===
    Et trobes en la zona de tir amic.
    Aquí podràs practicar la teva destresa amb les armes.
    Que pensaves que enviaríem a qualsevol a lluitar?
    Estaves ben equivocat, vinga comença.

- -> END

==cinem1==
  So… you found the book. #scene:1
  Inside lies the truth of what happened... and the price we paid to survive.#scene:2
  It began as a day like any other. Ships left the harbour; the Ohexa promised a plentiful day from the sea.#scene:3
  But deep in the shadows, a group of occultists was trying to summon a feral creature. Usually, their chants fall on deaf ears… But the universe was corrupted enough to make the impossible happen.#scene:4
  They made it; suddenly a big storm started, and an ominous kraken appeared in front of a ship… in no time all was destroyed.#scene:5
   Umiko, our Ohexa, thought about how to defeat the beast.#scene:6
   No mortal commanded the tides as she did… But the monster was stronger. #scene:7
   With her final breath, she bound the beast to her own soul. She saved us all, but at a terrible cost: she is doomed to guard the monster in the abyss for eternity.#scene:8
   When that happened, those pillars appeared; if they fall, the seal breaks, and the Kraken returns.#scene:9
   Destroy them and talk to the fisherman on the port; he will help you. Save Umiko, save us all… kill that monster.#scene:10 #changeScene:FINAL
- ->END

==cinemF==
    For the honour of the sea, the curse on this land must be defeated. We shall make you disappear among the sea of hope. Now, VANISH! #scene:1
    We made it. It’s been a long time, Umiko.\n Thanks, lord, you are alive… #scene:2
    You, the one that is the hero of this time… Thank you; without your help, defeating the monster would have been impossible.#scene:3
    Well, maybe my name will be written in history books, heh. Thank you… for saving us… for saving her… #scene:4
    Well, maybe my name will be written in history books, heh. Thank you… for saving us… for saving her… \n Don’t worry, darling. You protected us from that thing; you have done a good job… #scene:5
    I'm truly grateful to you; now I can’t make so much. after all this time sealing this creature, my power is weak… I think there is something I can give to you.#scene:6
    This is what you need to create runes; they will give you more power in battle. Use them wisely. Fisherman never learned how to create them… so don’t blame him for not showing them…#scene:7
    I'm sorry, as said, I always wanted a peaceful life…. \n You always wanted that, and I loved it. But now, the galaxy is corrupted, and you are the one that can save us. With this new power you will be able to do many new things#scene:8
    Keep going; your adventure must continue…. #scene:9 #changeScene:Main_menu
- ->END    