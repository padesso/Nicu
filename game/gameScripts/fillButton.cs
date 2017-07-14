function fillButton::onLevelLoaded(%this, %scenegraph)
{
   %this.useMouseEvents = true;
   
   // Lookup my image map and set the index that matches
   // my image map.  ($IM defined in fillGrid.cs.)
   for( %i = 0; %i < 6; %i++ )
   {
     if( %this.getImageMap() $= $IM[%i] )
       %this.imageMapIndex = %i;
   }  
}

function fillButton::onMouseDown(%this, %modifier, %worldPosition, %mouseClicks)
{   
   if($playerButtonStack.length > 0)
   {
      hideSelection($playerButtonStack.pop());
   }
   
   $playerButtonStack.push(%this);      

   showSelected(%this);
      
   echo("Player" SPC %this.Player SPC "clicked on color: " @ %this.getImageMap());
   
   //remove the mouse interaction - prevents a crash when double clicking a button
   setMouseControls(false);
   
   //fill with clicked color
   mainGrid.setCurrentColor(%this.Player, %this.imageMapIndex);
   
   //re-enable the mouse
   schedule(750, 0, setMouseControls, true);
   
   mainGrid.schedule( 700, computerPlay, 1 );
}

function getIndex(%imageMap)
{
   for(%i = 0; %i < 6; %i++)
   {
      if($IM[%i] $= %imageMap)
      {
         return %i;  
      }
   }
}

function setMouseControls(%val)
{
   greenFillButton.useMouseEvents = %val;
   orangeFillButton.useMouseEvents = %val;
   blueFillButton.useMouseEvents = %val;
   greenFillButton.useMouseEvents = %val;
   blackFillButton.useMouseEvents = %val;
   yellowFillButton.useMouseEvents = %val;
}