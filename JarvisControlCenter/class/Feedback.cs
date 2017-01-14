using System;

using Windows.Media.SpeechSynthesis;

namespace JarvisControlCenter
{
    internal class FeedbackClass
    {
      

        private static ConsoleLogInfos consoleLogInfos = new ConsoleLogInfos();

        private static SpeechSynthesizer jarvis = new SpeechSynthesizer();

        public async System.Threading.Tasks.Task<string> feedback(string name,string  action)
        {
            string retour = "";
            try
            {
                string tts = formatTTS(name, action);
                SpeechSynthesisStream ttsStream = await jarvis.SynthesizeTextToStreamAsync(tts);
                MainPage.jarvisTalk(ttsStream);
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error:", "feedback: " + ex.Message);
                
            }
            
       
            return retour;
        }


        public static string formatTTS(string name,string action) {
            string retour = "";

            string actionNumber = "";
            if (action == "on" || action == "On" || action == "ON")
            {
                actionNumber = "1";
            }
            else if (action == "off" || action == "Off" || action == "OFF")
            {
                actionNumber = "0"; // check if masculin ou féminin
            }

            string firstWord = findWord(name, "1");
            string secondWord = findWord(name, "2");

            retour = decryptTTS(name, actionNumber, firstWord, secondWord);
            consoleLogInfos.addLineToLogs("TTS: ", retour);
            return retour;
        }

        

        private static string findWord(string name, string wordLocation) {
            string retour = "false";
            char[] delimiterChars = {' '};

            try
            {
                string[] words = name.Split(delimiterChars);
                if (words[0] != "")
                {
                    if (wordLocation == "1") {
                        retour = words[0];
                    }
                    if (wordLocation == "2")
                    {
                        retour = words[1];
                    }
                }
            }
            catch (Exception ex)
            {
                consoleLogInfos.addLineToLogs("error: ", " findFirstWord : " + ex.Message);
                
            }
            
            return retour;
        }


        private static string decryptTTS(string name, string actionNumber, string firstWord, string secondWord)
        {
            string retour = "";
            string validate = "false";
            string verbe = "";
            string actionTTS = "";

            if (firstWord != "false")
            {
                if (firstWord == "le" || firstWord == "Le" || firstWord == "LE" || firstWord == "la" || firstWord == "La" || firstWord == "LA")
                {
                    verbe = "est";
                    if (firstWord == "le" || firstWord == "Le" || firstWord == "LE" || secondWord == "spot" || secondWord == "lustre" || secondWord == "Spot" || secondWord == "Lustre" || secondWord == "SPOT" || secondWord == "LUSTRE")
                    {
                        if (actionNumber == "1")
                        {
                            actionTTS = "allumé";
                            validate = "true";
                        }
                        if (actionNumber == "0")
                        {
                            actionTTS = "éteint";
                            validate = "true";
                        }
                    }

                    if (firstWord == "la" || firstWord == "La" || firstWord == "LA" || secondWord == "lumière" || secondWord == "Lumière" || secondWord == "LUMIERE")
                    {
                        if (actionNumber == "1")
                        {
                            actionTTS = "allumée";
                            validate = "true";
                        }
                        if (actionNumber == "0")
                        {
                            actionTTS = "éteinte";
                            validate = "true";
                        }
                    }
                }

                if (firstWord == "les" || firstWord == "Les")
                {
                    verbe = "sont";

                    if (secondWord == "spot" || secondWord == "lustre" || secondWord == "Spot" || secondWord == "Lustre" || secondWord == "SPOT" || secondWord == "LUSTRE") {
                        if (actionNumber == "1")
                        {
                            actionTTS = "allumés";
                            validate = "true";
                        }
                        if (actionNumber == "0")
                        {
                            actionTTS = "éteints";
                            validate = "true";
                        }
                    }

                    if (secondWord == "lumière" || secondWord == "Lumière" || secondWord == "LUMIERE")
                    {
                        if (actionNumber == "1")
                        {
                            actionTTS = "allumées";
                            validate = "true";
                        }
                        if (actionNumber == "0")
                        {
                            actionTTS = "éteintes";
                            validate = "true";
                        }
                    }



                }


            }


            if (validate == "true")
            {
                retour = "Voilà, " + name + " " + verbe + " " + actionTTS + " !";
            }
            else {
                retour = "OH! Oh!, il semble qu'il y a eu une erreur l'action à échouée !" ;
            }
           
            return retour;
        }


    } // Fin internal class FeedbackClass
}
