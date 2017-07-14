// Map an index to an image map string
$IM[0] = "greenPieceImageMap";
$IM[1] = "orangePieceImageMap";
$IM[2] = "bluePieceImageMap";
$IM[3] = "blackPieceImageMap";
$IM[4] = "redPieceImageMap";         
$IM[5] = "yellowPieceImageMap";  

/*NOTES: 

-Score board
--save to globals at round end
--prompt for name
--Submit scores to server
--pull back the 5 above and 5 below scores and display on screen

*/

function fillGrid::onLevelLoaded(%this, %scenegraph)
{   
   //fade the scene in
   $fadeEffect =  new t2dStaticSprite(blackFade) {
      imageMap = "BlackImageMap";
      scenegraph = %scenegraph; 
      frame = "0";
      useSourceRect = "0";
      sourceRect = "2.28084e-039 0 2.28084e-039 6.20144e-039";
      canSaveDynamicFields = "1";
      size = "100.000 75.000";
      Visible = 0;
      Layer = "1";
         mountID = "27";
   };
   
   $fadeEffect.setBlendAlpha(1);
   $fadeEffect.visible = 1;
   
   $fadeEffect.fadeOut(2500);//fade out   
   
   setupControls();

   //difficulty modifier
   if($difficulty $= "")    
      $difficulty = 4;  //start at 4 plies

   //cleanup the player name input gui
   if(   textInputGui.Visible == 1 )
      textInputGui.Visible = 0;

   $SG = %scenegraph;
   
   //used to show end of round values on second board & submit to server
   $endRoundScore = 0;   
   $endRoundClicks = 0; 
   $endRoundTime = 0;      //this should be in seconds to pass to server   
   $endRoundBigBurst = 0;
   
   $playerButtonStack = new ScriptObject(Stack);
   
   %this.startGame();
}

function fillGrid::startGame(%this)
{
   //enable the mouse controls - disabled when game ends
   setMouseControls(1);
   
   $timerOn = true;
   resetTimer();   
   startTimer();
   
   // Clone the tile map for a border map for each player
   %this.borders[0] = $SG.getGlobalTileMap().createTileLayer(
       %this.getTileCount(), %this.getTileSize() );
   %this.borders[0].setArea( %this.getArea() );
   %this.borders[0].setBlendColor( 0.63, 0.19, 0.01 );
   %this.borders[0].setLayer(2);
   
   %this.borders[1] = $SG.getGlobalTileMap().createTileLayer(
       %this.getTileCount(), %this.getTileSize() );
   %this.borders[1].setArea( %this.getArea() );
   %this.borders[1].setBlendColor( 0.12, 0.55, 0.68 );
   %this.borders[1].setLayer(2);
   
   %maxX = %this.getTileCountX();
   %maxY = %this.getTileCountY();
   
   %seed = getRandom(1, 1000000);   
   
   %this.model = new FillGridModel();
   %this.model.initialize( %maxX, %maxY, %seed );
      
   if(!%this.model.isBoardFair(10, 2))
   {
      echo("This board is not fair, restarting game.");
      %this.startGame();
   }
   else
   {
      echo("Starting game with seed: " @ %seed);
      
      %this.model = new FillGridModel();
      %this.model.initialize( %maxX, %maxY, %seed );
    
      %this.displayModel();   
      
      //set clicks
      %this.playerClicks[0] = 0;
      %this.playerClicks[1] = 0;
      
      player1ClicksLabel.text = %this.playerClicks[0];
      player2ClicksLabel.text = %this.playerClicks[1];
      $endRoundClicks = %this.playerClicks[0];
         
      difficultyLabel.text = $difficulty;
      timerLabel.text = "00:00";
   
      //%this.simulateGame();
   }
}

function fillGrid::onLevelEnded( %this, %scenegraph )
{
  %this.model.delete();
}

//called when user clicks a color
function fillGrid::setCurrentColor( %this, %player, %newColor )
{     
  playSE("click", 1);  
  %this.playerClicks[%player]++;
  
  %this.model.floodFill( %player, %newColor );
  %this.displayModel();
}

function fillGrid::displayModel( %this )
{
  %maxX = %this.getTileCountX();
  %maxY = %this.getTileCountY();
  
  for( %x = 0; %x < %maxX; %x++ )
  {
    for( %y = 0; %y < %maxY; %y++ )
    {
      %color = %this.model.color( %x, %y );
      %this.setStaticTile( %x SPC %y, $IM[%color] );
    }
  }
  
  %this.updateBorders();
  %this.setScores();
}

function fillGrid::updateBorders( %this )
{
  %maxX = %this.getTileCountX();
  %maxY = %this.getTileCountY();

  for( %x = 0; %x < %maxX; %x++ )
  {
    for( %y = 0; %y < %maxY; %y++ )
    {
      %owner = %this.model.owner( %x, %y );

      // If the tile isn't owned, we don't care about its borders.      
      if( %owner == -1 )
      {
        %this.borders[0].clearTile( %x, %y );
        %this.borders[1].clearTile( %x, %y );
        continue;
      }
      
      %bits = 0;
      
      %bits |= (%owner != %this.model.owner(%x, %y - 1)) << 0;
      %bits |= (%owner != %this.model.owner(%x + 1, %y)) << 1;
      %bits |= (%owner != %this.model.owner(%x, %y + 1)) << 2;
      %bits |= (%owner != %this.model.owner(%x - 1, %y)) << 3;
      
      %this.borders[%owner].setStaticTile(%x, %y, SquareBordersImageMap, %bits );      
    }
  }
}

function fillGrid::setScores(%this)
{
   //particle score effect
   %scoreDiff[0] =  %this.model.score(0) - player1OwnedLabel.text;
   %scoreDiff[1] =  %this.model.score(1) - player2OwnedLabel.text;
   
   if(%scoreDiff[0] > 0)
   {
      showScore(%scoreDiff[0], "-25, 35");
   }
   
   if(%scoreDiff[1] > 0)
   {
      showScore(%scoreDiff[1], "33, 35");
   }
   
  player1ClicksLabel.text = %this.playerClicks[0];
  player2ClicksLabel.text = %this.playerClicks[1];
  $endRoundClicks = %this.playerClicks[0];
  
  player1OwnedLabel.text = %this.model.score(0);  
  player2OwnedLabel.text = %this.model.score(1);
  $endRoundScore = %this.model.score(0);  
  
  if(%scoreDiff[0] > %this.bigBurst[0])
  {
      %this.bigBurst[0] = %scoreDiff[0];
  }
  
  if(%scoreDiff[1] > %this.bigBurst[1])
  {
      %this.bigBurst[1] = %scoreDiff[1]; 
  }
  
  player1BigBurstLabel.text = %this.bigBurst[0];
  player2BigBurstLabel.text = %this.bigBurst[1];  
  $endRoundBigBurst = %this.bigBurst[0];
  
  //Game over
  if(%this.model.score(0) + %this.model.score(1) >= %this.getTileCountX() * %this.getTileCountY())
  {
      //disable the mouse controls so player can't keep clicking
     setMouseControls(0);
     
      stopTimer();
      $endRoundTime = $startSeconds;
      
      removeControls();     
      
      $fadeEffect.schedule(2750, "fadeIn", 1250, 1);//fade in   
      
      //show round end message
      sceneWindow2D.schedule(4000,"loadLevel","game/data/levels/playAgainMenu.t2d");      
  }
}

function fillGrid::simulateGame(%this)
{
   if(%this.model.score(0) + %this.model.score(1) < %this.getTileCountX() * %this.getTileCountY())
   {
      %this.schedule( 32, computerPlay, 0 );
      %this.schedule( 512, computerPlay, 1 ); 
      %this.schedule( 1024, simulateGame); 
   }
}

function fillGrid::computerPlay( %this , %player)
{
  %move = %this.model.bestMove( %player, $difficulty );
  
  if( %move == -1 ) 
   return;
  
  %this.setCurrentColor( %player, %move );
}

function setupControls()
{
   //set the pause key active
   moveMap.bindCmd(keyboard, escape, "togglePause();", "");
   
   //move the selection  
   moveMap.bindCmd(keyboard, a, "selectPrev(0);", "");
   moveMap.bindCmd(keyboard, z, "selectNext(0);", "");
   
   moveMap.bindCmd(keyboard, up, "selectPrev(1);", "");
   moveMap.bindCmd(keyboard, down, "selectNext(1);", "");
   
   moveMap.bindCmd(keyboard, space, "fireSelected();", "");
}

function selectPrev(%player)
{
   //TODO
}

function selectNext(%player)
{
   //TODO
}

function fireSelected()
{
   //TODO 
}

function removeControls()
{
   moveMap.unbind(keyboard, escape);
    
   moveMap.unbind(keyboard, a);
   moveMap.unbind(keyboard, z);
   
   moveMap.unbind(keyboard, up);
   moveMap.unbind(keyboard, down);
   
   moveMap.unbind(keyboard, space);
}