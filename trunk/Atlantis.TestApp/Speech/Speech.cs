namespace Atlantis.TestApp.Speech
{
    using Atlantis.Enterprise.Voice;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Speech;
    using System.Speech.Synthesis;

    public class Speech
    {
        #region Methods

        public static void Main(string[] args)
        {
            Console.Title = "Voice Testing";

            //String passwd = Security.GenerateVoicePassword("Gunnett", 3);
            //String janeway = Security.GenerateVoicePassword("Janeway", 3);

            //Console.WriteLine("Generated Voice Password: {0}", passwd);

            SpeechSynthesizer synth = new SpeechSynthesizer();
            //synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Child, 3);
            //synth.Speak(passwd);

            //Console.WriteLine("Generated Voice Password: {0}", janeway);

            /*PromptBuilder pb = new PromptBuilder();
            synth.Speak(pb);*/
            //synth.Speak(janeway);


            String[] names = { "T'Pol", "Janeway", "Gunnett", "Loveless", "Archer", "Picard", "Riker", "Anderson" };
            foreach (var item in names)
            {
                String passwd2 = Security.GenerateVoicePassword(item, 3);
                Console.WriteLine("Generated Voice Password: {0}", passwd2);
                synth.Speak(passwd2);
            }


            Console.Write("Press any key to continue. . .");
            Console.ReadKey(true);
        }

        #endregion
    }
}
