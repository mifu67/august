INCLUDE globals.ink

===desk===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(So this is August's desk.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
-> END
=julian
So this is August's desk. #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
You know, it's messier than I thought it would be. #speaking:speaker2 #portrait2:julian-happy
I always thought that he was a neat freak. 
Heh. #speaking:speaker1 #portrait1:erika-happy
He usually is, but when he's busy...
Or, he was neat, I guess. He's dead now. #portrait1:erika-sad
... Let's take a look, shall we? #portrait1:erika-neutral
-> END

===papers===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(This is a list of suicides in Larsen over the past six months.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
(I guess they'll put August on this list.)
(Three names are highlighted. Karen Wang, Lily Samuelsen, and Anna Arvin.) #portrait1:erika-neutral
(Hmm.)
-> END
=julian
This is a list of suicides in Larsen over the past six months. #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
F*ck. There are so many. #speaking:speaker2
Maybe the sh*tty weather here just makes everyone want to kill themselves. 
... #speaking:speaker1
Sorry. #speaking:speaker2
No, I get it. #speaking:speaker1
Anyway. I guess they'll put August on this list. One more for the statistic. #portrait1:erika-sad
Damn. #speaking:speaker2 #portrait2:julian-angry
Wait, three of these names are highlighted. #portrait2:julian-neutral
Yeah, I’m just noticing that. Karen Wang, Lily Samuelsen, and Anna Arvin. #speaking:speaker1 #portrait1:erika-neutral
Interesting.
-> END 

===file===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(A case file. I shouldn't look, but... well. I'm going to look.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy 
(“Karen Wang. Age: 25. Cause of death: direct shot to the right temple with a 0.44 caliber pistol. Presumed to be self-inflicted.”)
(“Notes: victim suffered from depression. According to mother, she was taking medication and seemed to be getting better. Wang’s mother says her daughter’s suicide was completely unexpected.”)
-> END
=julian
This is a case file. We shouldn't look, but— #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
Let's open it. #speaking:speaker2
“Karen Wang. Age: 25. Cause of death: direct shot to the right temple with a 0.44 caliber pistol. Presumed to be self-inflicted.” #speaking:speaker1
“Notes: victim suffered from depression. According to mother, she was taking medication and seemed to be getting better. Wang’s mother says her daughter’s suicide was completely unexpected.”
God damn. Her poor mother. For your daughter to be getting better, and then.... #speaking:speaker2 #portrait2:julian-angry
Yeah. You'd always be left wondering what went wrong. #speaking:speaker1 #portrait1:erika-sad
-> END

===dino===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(It's a bobblehead figurine of a dinosaur.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
(I didn't even know that he had this. It's so cute.)
-> END
=julian
This is so cute. I didn't even know that he had this. #speaker1:Erika #speaker2:Julian #portrait1:erika-happy #portrait2:julian-neutral #speaking:speaker1 #color:black
... #speaking:speaker2
August's favorite dinosaur is the plesiosaurus.
It is? #speaking:speaker1 #portrait1:erika-neutral
Yeah. We used to own this big book of dinosaurs. He would read it to me when I was little. #speaking:speaker2
I didn't know. I never asked him. #speaking:speaker1
Now I wish that I had. #portrait1:erika-sad
-> END

===drawer===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(Let's open this, shall we?) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
-> END
=julian
Let's open this, shall we? #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
-> END

===ring===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(August... I would have said yes a million times.) #speaker1:Erika #portrait1:erika-sad #speaking:speaker1 #color:navy
-> END
=julian
...#speaker1:Erika #speaker2:Julian #portrait1:erika-sad #portrait2:julian-neutral #speaking:speaker1 #color:black
Maybe this is what he wanted to show me yesterday. #speaking:speaker2
I'm so sorry.
Of course I would have said yes. #speaking:speaker1
I—
I wish we could have grown old together.
But he'll be 27 forever, and in a year, I'll be older than he ever was.
F*ck. I didn't even think about that. #speaking:speaker2 #portrait2:julian-angry
God damn it. This is so sh*tty.
Why did he have to f*cking die, Erika?
-> END

===gun===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(This must be August's gun. But what is it doing here?) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
-> END
=julian
This must be August's gun. #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
What is it doing here? I thought he shot himself in the head. #speaking:speaker2 #portrait2:julian-angry
That's what the police are saying. #speaking:speaker1 #portrait1:erika-sad
(Hmm....) #portrait1:erika-neutral #color:navy
-> END

===mug===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(It's still stained with yesterday's dregs.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
-> END
=julian
It's still stained with yesterday's dregs. #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
-> END

===notes===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(Hmm? What's this?) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
("Victim: Laurier, August Jean. Sex: M. Age: 28.")
(The case notes for August's death? Did someone drop them here?)
(Wait...)
(Something about this report... I should save this and take a closer look at it later.)
<b>Police report added to evidence.</b #color:black
-> END
=julian
Hmm? What's this? #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
"Victim: Laurier, August Jean. Sex: M. Age:28."
The case notes for August's death? Did someone drop them here?
We should just take them. I mean, they were on the floor, so it's not like we're stealing classified information, right? #speaking:speaker2
Okay. Wait a second... #speaking:speaker1
What? Everything good? #speaking:speaker2
Somthing about this report... we should take a closer look at it later. #speaking:speaker1
<b>Police report added to evidence.</b> #color:black
-> END