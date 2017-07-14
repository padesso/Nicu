$markerRadialSpeed = 15;



function selectionMarker::onLevelLoaded( %this, %scene )
{
   %this.isSelected = false;
}

function selectionMarker::onUpdate( %this )
{   
   //%this.currentAngle += %this.angleDelta;
   %this.currentAngle += %this.angleDelta * 0.0075;
   %center = %this.center;
   %vector = mCos( %this.currentAngle ) SPC mSin( %this.currentAngle );
   %vector = t2dVectorScale( %vector, %this.radius );
   %vector = t2dVectorAdd( %vector, %center );
   %this.setPosition( %vector );
}

//function clickableObject::onMouseUp(%this, %modifier, %worldPosition, %clicks)
function showSelected(%obj)
{
   //echo("Width of clicked object:" SPC %obj.getWidth());
   
   //get the radius for the selected item
   %obj.targetRadius = 0.5 * t2dGetMax( %obj.getWidth(), %obj.getHeight() ); 
   
   //figure our how many marker symbols should be created (cast radius as an int)
   %obj.numMarkers = mRound(%obj.targetRadius) + 2;
   
   //create the marker symbols
   for(%i = 0; %i < %obj.numMarkers; %i++)
   {
      //store the animated sprites in an array so we can delete them later
      %obj.markers[%i] = new t2dAnimatedSprite()
      {
         class = "selectionMarker";
         Position = "5000 5000";
         visible = 0;
         animationName = "particles1Animation";
         size = "5.000 5.000";
         currentAngle = 0 + (%i / %obj.numMarkers) * (2 * 3.14);
         radius = %obj.targetRadius  + (5 * getRandom());
         center = %obj.getPosition();     
         //angleDelta = 0.04;
         angleDelta = $markerRadialSpeed / %obj.targetRadius + (2 * getRandom());  
         scenegraph = %obj.getSceneGraph();   
         Layer = 2;  
      };
      
      //make the markers appear sequentially
      %obj.markers[%i].schedule( %i * 2500 / %obj.numMarkers, setVisible, true );
      
      %obj.markers[%i].enableUpdateCallback();
      
      %obj.markers[%i].setBlendAlpha(0);
      %obj.markers[%i].fadeIn(500 + (150 * getRandom()), 0.15 + (0.65 * getRandom()));    
      
      
      //make the markers "march"
      %obj.markers[%i].setAnimationFrame(%i);
   }
}

function hideSelection(%obj)
{
   %obj.isSelected = false;
   
   //delete the markers attached to this object
   for(%j = 0; %j < %obj.numMarkers; %j++)
   {
      //make the markers disappear sequentially
      %obj.markers[%j].schedule( %j * 1500 / %obj.numMarkers, safeDelete);
   }
}