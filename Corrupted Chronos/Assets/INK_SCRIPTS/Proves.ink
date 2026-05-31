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
   Destroy them and talk to the fisherman on the port; he will help you. Save Umiko, save us all… kill that monster.#scene:10 #changeScene:SampleScene
- ->END

==cinemF==
    For the honour of the sea, the curse on this land must be defeated. We shall make you disappear among the sea of hope. Now, VANISH! #scene:1
    We made it. It’s been a long time, Umiko. Thanks, lord, you are alive… #scene:2
    You, the one that is the hero of this time… Thank you; without your help, defeating the monster would have been impossible.#scene:3
    Well, maybe my name will be written in history books, heh. Thank you… for saving us… for saving her… #scene:4
    Well, maybe my name will be written in history books, heh. Thank you… for saving us… for saving her… #scene:5
    Don’t worry, darling. You protected us from that thing; you have done a good job… #scene:5
    I'm truly grateful to you; now I can’t make so much. after all this time sealing this creature, my power is weak… I think there is something I can give to you.#scene:6
    This is what you need to create runes; they will give you more power in battle. Use them wisely. #scene:7
    Fisherman never learned how to create them… so don’t blame him for not showing them…#scene:7
    I'm sorry, as said, I always wanted a peaceful life…. \n You always wanted that, and I loved it.#scene:8
    But now, the galaxy is corrupted, and you are the one that can save us. With this new power you will be able to do many new things.#scene:8
    Keep going; your adventure must continue…. #scene:9 #changeScene:Main_menu
- ->END 
// --- VARIABLES ---
VAR pillars_broken = 0  //Update by code
VAR met_fisherman = false

=== fisherman ===
{
    - pillars_broken == 0:
        -> zero_pillars
    - pillars_broken == 1:
        -> one_pillar
    - pillars_broken == 2:
        -> two_pillars
    - pillars_broken == 3:
        -> three_pillars
    - pillars_broken == 4:
        -> four_pillars 
}
// --- PILLAR 0 ---
=== zero_pillars ===
{ met_fisherman:
    Come on, go. HAHAHA.
    -> END
}
~ met_fisherman = true
The keeper talked to me about your plan… You are brave if you think you can defeat that guy.
Maybe I’m just too foolish to know when I’m outmatched. A hero’s got to start somewhere, right?
HAHAHA, I like you. If you can crack a joke with the abyss staring you in the face, you might just have a chance.
I will do my best.
Go for it, break the seals; I’ll wait for you here.
Don’t let those fishes escape; I want to eat some when this ends.
HAHAHA.
~ met_fisherman = true
-> END

// --- PILLAR 1 ---
=== one_pillar ===
{ not met_fisherman:
    You made it?
    I destroyed the biggest pillar in history!
    Haha, I’m sorry to tell you this, but all have the same size?
    HUH? Are you kidding me? All the same size?
    Oh lord, and you are the spark of hope we all need? Come on go destroy the others.
    HAHAHA, don’t worry, I will make it; I just came to check if you were alright.
    Don't worry about me and go.
    -> END
}
~ met_fisherman = true
The keeper talked to me about your plan… You are brave if you think you can defeat that guy.
Or a fool that destroyed one accumulation of rocks while pretending to know what was being made. A true hero!
Accumulation of rocks? Oh, HAHAHA, you already broke one of the pillars. I like you; something tells me you will make it.
I will try at least.
There are already two more rocks to be broken; come on, go.
Of course, man, and don’t let those fishes escape; I want to eat some when this ends.
HAHAHA.
~ met_fisherman = true
-> END

// --- PILLAR 2 ---
=== two_pillars ===
{ not met_fisherman:
    You are here again; you made it?
    There is one that escapes my sight. I think that one is playing hide and seek.
    You know… Rocks can’t move?
    But those are special, aren’t they? So… maybe they… move?
    Trust me, “hero”, they don’t move. Come on, move your spaceship and complete the job.
    Okay, okay, don’t get angry… See you!
    -> END
}
~ met_fisherman = true
The keeper talked to me about your plan… You are brave if you think you can defeat that guy.
I'm just the strongest hero you can find and the most handsome. Already destroyed two… what are they called… waterpillars? Well, those rocks on the water…
HAHAHA, joking about this situation with death upon your soul. You have my support; you have a chance against the beast.
I will work hard for it.
There is one more caterpillar there. Go on, I’ll wait for you here.
Thank you, and don’t let those fishes escape; I want to eat some when this ends.
HAHAHA.
~ met_fisherman = true
-> END

// --- PILLAR 3 (FINAL) ---
=== three_pillars ===
You are here. Have you destroyed all three pillars?
May the chronos bless you as the time to suffer in battle has come; oh lord, offer us your power.
Huh? What are you blabbing about, kid?
Well, they have always told me to say something epic before an important battle… Was it too much?
HAHAHA, you truly know how to break the tension.
Mission accomplished, heh. Now, the work comes in. I’ve destroyed all the pillars. It’s time.
So, you made it… Tell me, are you afraid?
Yes, I’d be an idiot not to be; that creature is a nightmare.
It really is, but the sea waited for so long to get the freedom you will provide. It can tell you are the saviour in that corrupted chronos we all live in.
You can talk to the sea? How long have you been alone?
HAHAHA, you may think I just lost my mind, but I'm a water ohexa as Umiko was. We can sense in a very precise way the element we have affinity with. 
That’s why I made myself a fisherman; there is no bad fishing day, HAHAHA.
But you can use your power for something more… heroic?
That’s true, boy, but I never wanted that. I love peaceful days. I don’t want my name to be remembered. 
Certainly this power was given to me by fault…
Or maybe this is what you think. As far as I know, you will help me summon the Kraken again. 
All was written; you needed to be here at this moment.
I don't believe in fate, but perhaps you are right. You need me to call the beast out of the seal…
You don’t need to take big fights to be considered a hero. Don’t blame yourself for having this power. 
From what I can tell, eating is also important, and having someone that will always provide food will be a very useful way of power usage. Not all is war.
Thank you. You are such a hero, aren't you?
HAHAHA, I just try to get the morale up. We have a tough fight to do.
HYes, here we go. Are you ready?
Ready!
"Great lord of the ocean, hear our chants falling for the star upon us, the ones that will set this corruption away are imploring you, OPEN YOUR DOORS!” #BOSS
~ pillars_broken = 4
-> END
=== four_pillars ===
GO
Yes yes, I'm just scared
->END
