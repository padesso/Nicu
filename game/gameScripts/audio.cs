$SOUND = true;
$VOLUME = 1.0;

new AudioDescription(SE)
{
   volume   = 1.0;
   isLooping = false;
   isStreaming = false;
   is3D     = false;
   type     = $GuiAudioType;
};

new AudioDescription(Music)
{
   volume   = 1.0;
   isLooping = true;
   isStreaming = false;
   is3D     = false;
   type     = $GuiAudioType;
};

function getAudio(%name, %type)
{
   if (isObject(%name))
      return;
   
   new AudioProfile(%name)
   {
      filename = "~/data/audio/" @ %name @ ".ogg";
      description = %type;
      preload = false;
   };
}

function playSE(%sound, %volume)
{
   // get profile for sound effect
   getAudio(%sound, "SE");    
   
   if($SOUND)
   {
      // play sound
      SE.volume = %volume;
      $se = alxPlay(%sound);
   }
}

function playMusic(%sound, %volume)
{
   // get profile for sound effect
   getAudio(%sound, "Music");    
   
   if($SOUND)
   {
      // play sound
      Music.volume = %volume;
      $music = alxPlay(%sound);
   }
}