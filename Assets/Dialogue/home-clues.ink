INCLUDE globals.ink

===scissors===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
 =no_julian
(Left handed scissors. These must be August’s.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
(I wonder what he was cutting....)
-> END
=julian
Left handed scissors. These must be August’s. #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
What was he cutting?
Probably red string. You know, like the detectives do in the movies? #speaking:speaker2
I wonder if he’s got one of those crazy pushpin boards hidden away somewhere. #portrait2:julian-happy
-> END

===plant===
(This is dying. Did I overwater it? Did I forget to water it? Who knows. Nothing matters anymore.) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy 
-> END

===bed===
(I don’t think I’ll be able to sleep tonight.) #speaker1:Erika #portrait1:erika-sad #speaking:speaker1 #color:navy
-> END

===photo===
{   talked_to_julian:
        -> julian
    - else:
        -> no_julian
}
=no_julian
(We took this at the bus stop where we first met. I can't believe that was five years ago....) #speaker1:Erika #portrait1:erika-neutral #speaking:speaker1 #color:navy
-> END
=julian
We took this at the bus stop where we first met. I can't believe that was five years ago.... #speaker1:Erika #speaker2:Julian #portrait1:erika-neutral #portrait2:julian-neutral #speaking:speaker1 #color:black
-> END