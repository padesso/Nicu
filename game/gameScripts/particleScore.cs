//Usage
//showScore(1234, "0 25");

//score should be the numbers only!!!
function showScore(%score, %startPos)
{
   //echo("Showing score: " @ strlen(%score));  
   
   for(%i = 0; %i < strlen(%score); %i++)
   {
      %xPos = getWord(%startPos, 0) + (%i * 5);
      %yPos = getWord(%startPos, 1);
      %newPos = %xPos SPC %yPos;
      
      playScoreEffect(getSubStr(%score,%i,1), %newPos);  
   }
}

function playScoreEffect(%score, %pos)
{
   %scoreEffect = new t2dParticleEffect() 
   {
     scenegraph = $SG;   
   };

   %particlePath = "~/data/particles/Score.eff";

   %scoreEffect.loadEffect(%particlePath);
   %scoreEffect.setEffectLifeMode("KILL", 2);
   %scoreEffect.setPosition(%pos);
   
   %emitter = %scoreEffect.getEmitterObject(0);
   %emitter.setImageMap("numbersLinkImage", %score);
   
   %scoreEffect.playEffect();   
}

function t2dParticleEffect::onStopEffect(%this)
{
   //echo("deleting");
   
   %this.safeDelete();
}