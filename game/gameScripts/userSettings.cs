//TODO: User settings
/*

Save last input name
Save last played difficulty
*/

/*
writeSettings("NAME","1");
//saves to:
C:\Users\Patrick\AppData\Roaming\Dream. Build. Repeat\Fill Battle\game\gameScripts\game\data\settings.txt
*/

//pass this presorted, comma seperated lists of scores and names
function writeSettings(%name,%difficulty)
{
  %fileObj = new FileObject();

   if(!%fileObj.openForWrite( "./game/data/settings.txt" ))
   {
      %fileObj.delete();
      echo("Failed to open file for write!");
      return -1;
   }
   
   echo("Succesfully opened FileObject for write");
   
   //write the names and scores to the file
   %fileObj.writeLine(%name);
   %fileObj.writeLine(%difficulty);
   
   %fileObj.close();
   
   %fileObj.delete();
   
   echo("Succesfully wrote to and closed FileObject");
   
   return 1;
}

function readSettings() 
{   
   %fileObj = new FileObject();
   
   // Test reading from the file we just wrote
   if(!%fileObj.openForRead( "./game/data/settings.txt" ))
   {
      %fileObj.delete();
      return "Player|4";//Failed to open file for read after succesful write!";
   }

   while( !%fileObj.isEOF() )
   {
      //seperate the name and difficulty
      %result = %result @ %fileObj.readLine() @ "|"; 
   }   

   %fileObj.delete();
   
   return %result;
   //TODO: set difficulty and user name
}

