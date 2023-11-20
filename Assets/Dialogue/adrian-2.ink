VAR num_reasons_explored = 0

===intro===
Adrian? #speaking:erika #portrait:adrian-neutral
Oh, Erika. You're back. #speaking:npc
How are you holding up?
Still terrible. But.... #speaking:erika
You found something. #speaking:npc #portrait:adrian-angry
Yes. I did. #speaking:erika
So tell me. Did August commit suicide? #speaking:npc #portrait:adrian-neutral
-> END

===yes_suicide===
I— #speaking:npc #portrait:adrian-angry
I see.
I never even knew that he was struggling. God damn it. August....Erika, I am so, so sorry.
... #speaking:erika
I don’t know what to say. I'm sorry. If there’s anything I can do for you, let me know. #speaking:npc
It's okay, Adrian. #speaking:erika
I'm going to leave now, if that's alright.
Of course. Goodbye, Erika. #speaking:npc
-> END

===no_suicide===
F*ck. I had a suspicion that was the case. #speaking:npc #portrait:adrian-angry
What did you find?
-> END

===reason_1===
{reason_1 <= 1:
    ~ num_reasons_explored += 1
}
Oh my God. You’re right. He is left handed. He wears his watch on his right wrist, he always has a huge ink smudge on the left side of his left hand, I’ve seen him writing a billion f*cking times…I didn’t even notice. #speaking:npc #portrait:adrian-angry
I’ve been up since 2… I must have been so tired that I missed it.
One moment—let me consult the report—yes, here it is. "He was found with the gun in his right hand".
So that settles it. Not knowing that he was left handed, someone shot August at close range and manipulated the scene to make it look like a suicide.
-> END

===reason_2===
{reason_2 <= 1:
    ~ num_reasons_explored += 1
}

{ num_reasons_explored == 1:
I found August's revolver in his desk. He didn't have it with him when he died. #speaking:erika #portrait:adrian-neutral
Of course he left the gun. He never brought it anywhere. He was always a lousy shot.... #speaking:npc 
That’s a good point. If he was going to kill himself, he probably would have used his own gun... but that argument alone isn’t enough for me to disobey the chief’s orders against investigating his death.
There's a possibility that he bought a different gun, or that he used someone else’s gun. 
Have you discovered anything else that suggests that August didn’t commit suicide?
}
I found August's revolver in his desk. He didn't have it with him when he died. #speaking:erika #portrait:adrian-neutral
Of course he left the gun. He never brought it anywhere. He was always a lousy shot.... #speaking:npc 
That’s a good point. If he was going to kill himself, he would have used his own gun.
I suppose he could have bought a different gun or used someone else's gun, but with the other information you've given me, it's too suspicious. 
I believe you, Erika. I think that someone shot August at close range and manipulated the scene to make it look like a suicide. #portrait:adrian-angry
-> END

===reason_3===
{reason_3 <= 1:
    ~ num_reasons_explored += 1
}

{ num_reasons_explored == 1:
August was investigating the suicide of a woman named Karen Wang. He died in the same manner as her. #speaking:erika #portrait:adrian-neutral
Karen Wang... that sounds familiar. Let me look at her file.... Okay, here it is. #speaking:npc
“Karen Wang. Age: 25. Cause of death: direct shot to the right temple with a 0.44 caliber pistol. Presumed to be self-inflicted.” Damn. This exactly matches the way that August died. #portrait:adrian-angry
It’s troubling, but that argument alone isn’t enough for me to disobey the chief’s orders against investigating his death. 0.44 caliber pistols are the most common make used by the Larsen Police Force—we can’t rule out sheer coincidence here. 
Have you discovered anything else that suggests that August didn’t commit suicide? #portrait:adrian-neutral
}
August was investigating the suicide of a woman named Karen Wang. He died in the same manner as her. #speaking:erika #portrait:adrian-neutral
Karen Wang... that sounds familiar. Let me look at her file.... Okay, here it is. #speaking:npc
“Karen Wang. Age: 25. Cause of death: direct shot to the right temple with a 0.44 caliber pistol. Presumed to be self-inflicted.” Damn. This exactly matches the way that August died. #portrait:adrian-angry
I suppose it could have been sheer coincidence, but with the other information you've given me, it's too suspicious.
I believe you, Erika. I think that someone shot August at close range and manipulated the scene to make it look like a suicide.
-> END

===reason_4===
{reason_4 <= 1:
    ~ num_reasons_explored += 1
}
{ num_reasons_explored == 1:
August had made future plans with Julian and our neighbor, Mr. Palomino, before he died. #speaking:erika #portrait:adrian-neutral
Hmm. It is suspicious that he would make plans if he knew he would be dead. Couple that with the fact that none of us thought that August was suicidal.... #speaking:npc
It’s troubling, but that argument alone isn’t enough for me to disobey the chief’s orders against investigating his death. As much as I hate to admit it, there's a possibility that he could have deceived us all. #portrait:adrian-angry
Have you discovered anything else that suggests that August didn’t commit suicide? #portrait:adrian-neutral
}
August had made future plans with Julian and our neighbor, Mr. Palomino, before he died. #speaking:erika #portrait:adrian-neutral
It is suspicious that he would make plans if he knew he would be dead. Couple that with the fact that none of us thought that August was suicidal.... #speaking:npc
I suppose there's a possibility that he deceived us all, but with the other information you've given me, it's all too strange. 
I believe you, Erika. I think that someone shot August at close range and manipulated the scene to make it look like a suicide. #portrait:adrian-angry
-> END
