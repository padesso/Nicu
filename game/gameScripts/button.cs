function Button::onMouseDown(%this, %modifier, %worldPosition, %mouseClicks)
{
   echo("Player clicked on: " @ %this.name);
   
   //remove the mouse interaction - prevents a crash when double clicking a button
   %this.useMouseEvents = false;
   
   $fadeEffect.schedule(500, "fadeIn", 2250, 1);//fade in      
   
   if(%this.getName() $= "yesButton")
   {   
      if($isTwoPlayer)
      {
         //schedule(2500, 0, "startGame", "expandFilename", "game/data/levels/twoPlayerGame.t2d" );
         sceneWindow2D.schedule(2500,"loadLevel","game/data/levels/twoPlayerGame.t2d");
      }
      else
      {
         sceneWindow2D.schedule(2500,"loadLevel","game/data/levels/mainGame.t2d");
      }
   }
   else if(%this.getName() $= "noButton")
   {
      Canvas.schedule(2500, "setContent" ,MainMenuGui);
      textInputGui.schedule(2500, "setVisible", true);
   }
}

function webLink::onMouseUp(%this)
{
   gotoWebPage( "http://www.dreambuildrepeat.com/FillBattle/FillBattle.html" );
}