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

            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Child, 3);// there's only one installed ....

            /*foreach (var voice in synth.GetInstalledVoices())
            {
                Console.WriteLine("Voice: ID[{0}] Name[{1}] Gender[{2}] Age[{3}]",
                    voice.VoiceInfo.Id,
                    voice.VoiceInfo.Name,
                    voice.VoiceInfo.Gender.ToString(),
                    Enum.GetName(voice.VoiceInfo.Age.GetType(), voice.VoiceInfo.Age));

                foreach (KeyValuePair<string, string> kvp in voice.VoiceInfo.AdditionalInfo)
                {
                    Console.WriteLine("\tKey: {0} - Value: {1}", kvp.Key, kvp.Value);
                }
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();*/

            List<string> names = new List<string>()
            {
                "Janeway", "Gunnett", "Loveless", "Archer",
                "Picard", "Riker", "Anderson", "Troi",
                "Crusher", "O'Neil", "Data", "Worf",
                /*
                "Jackson", "Carter", "Parrett", "Bartling",
                "Milano", "Corley", "Mayr", "Steeves",
                "Durbin", "Buckholz", "Mercure", "Davidson",
                "Legree", "Hairston", "Hoge", "Hayman",
                "Wales", "Shirey", "Leist", "Lennon",
                "Coon", "Killgore", "Phalen", "Collinsworth",
                "Simard", "Plude", "Filson", "Vise",
                "Towner", "Gosier", "Ehrmann", "Barnum",
                "Searight", "Dumont", "Dicarlo", "Vallery",
                "Cullison", "Parlier", "Negron", "Blumer",
                "Karcher", "Posey", "Vantassell", "Storlie",
                "Rott", "Iman", "Tibbitts", "Dressel",
                "Bochenek", "Bath", "Pillot", "Brousseau",
                 */
            };

            foreach (string name in names)
            {
                string passwd = Security.GeneratePassword(name);
                Console.WriteLine("Generated Voice Password: {0}", passwd);
                synth.Speak(passwd);
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);

            synth.Dispose();
        }

        #endregion
    }
}
