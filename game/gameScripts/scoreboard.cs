//TCPSubmitScore("Patrick", "72", "13", "1:07", "22");

function TCPSubmitScore(%name, %score, %clicks, %time, %bigBurst)
{
	%obj = new TCPObject(TCPScoreSender);
	%obj.name = %name;
	%obj.score = %score;
	%obj.clicks = %clicks;
	%obj.time = %time;
	%obj.bigBurst = %bigBurst;
	
	%obj.connect("www.dreambuildrepeat.com:80");
}

function TCPScoreSender::onConnected(%this)
{
   // add the correct tags for the php to read the name and score correctly
   %data = 
      "&name=" @ %this.name @ 
      "&score=" @ %this.score @ 
      "&clicks=" @ %this.clicks @
      "&time=" @ %this.time @ 
      "&bigBurst=" @ %this.bigBurst;
	
	%data = URLEncode(%data);
		
	%httpCmd="POST /FillBattle/FillBattle.php HTTP/1.1\nHost: www.dreambuildrepeat.com:80\nUser-Agent: Torque/1.0 \nAccept: */*\nContent-Length: "@ strlen(%data) @"\nContent-Type: application/x-www-form-urlencoded; charset=UTF-8\n\n" @ %data;

	echo(%httpCmd);
	%this.send(%httpCmd @ " \r\n");
}

function URLEncode(%rawString, %excludeChars)
{
    if (!isDefined("%excludeChars"))
        %excludeChars = "";

    // Encode strings to be HTTP safe for URL use   

    // Table of characters that are valid in an HTTP URL
    %validChars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz:/.?=_-$(){}~&";


    // If the string we are encoding has text... start encoding
    %len = strlen(%rawString);
    if (%len > 0)
    {
        // Loop through each character in the string
        for(%i=0; %i < %len; %i++)
        {
            // Grab the character at our current index location
            %chrTemp = getSubStr(%rawString,%i,1);

            //  If the character is not valid for an HTTP URL... Encode it        
            if (strstr(%validChars, %chrTemp) == -1 || strstr(%excludeChars, %chrTemp) != -1)
            {
                //Get the HEX value for the character
                %chrTemp = dec2hex(chrValue(%chrTemp));

                // Is it a space?  Change it to a "+" symbol
                if (%chrTemp $= "20")
                {
                    %chrTemp = "+";
                }
                else
                {
                    // It's not a space, prepend the HEX value with a % 
                    %chrTemp = "%" @ %chrTemp;
                }       
            }
            // Build our encoded string
            %encodeString = %encodeString @ %chrTemp;
        }
    }
    // Return the encoded string value
    return %encodeString;
}

//this is fired when the do you want to play agian screen shows
function highScoreStarter::onLevelLoaded(%this, %scenegraph)
{
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
   
   $fadeEffect.fadeOut(750);//fade out 
   
   //TODO: check for internet connection
   
   endBurstLabel.text = $endRoundBigBurst;
   endClicksLabel.text = $endRoundClicks;
   endScoreLabel.text = $endRoundScore;
   endTimeLabel.text = $roundTime;   
   
   if(!$isTwoPlayer)
   {
      schedule(150, 0, TCPSubmitScore, $playerName, $endRoundScore, $endRoundClicks, $roundTime, $endRoundBigBurst);
   }
}