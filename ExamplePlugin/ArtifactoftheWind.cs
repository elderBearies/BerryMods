using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using static BerryMods.BerryMods;
namespace BerryMods
{
	class FlightArtifact : ArtifactBase
	{
		public override string ArtifactName => "Artifact of the Wind";
		public override string ArtifactLangTokenName => "ARTIFACT_OF_FLIGHT";
		public override string ArtifactDescription => "Everything flies.";
		public override Sprite ArtifactEnabledIcon => MainAssets.LoadAsset<Sprite>("RoR2FlightActive.png");
		public override Sprite ArtifactDisabledIcon => MainAssets.LoadAsset<Sprite>("RoR2FlightInactive.png");
		public override void Init(ConfigFile config)
		{
			CreateConfig(config);
			CreateLang();
			CreateArtifact();
			Hooks();
		}
		private void CreateConfig(ConfigFile config)
		{
			//TimesToPrintMessageOnStart = config.Bind<int>("Artifact: " + ArtifactName, "Times to Print Message in Chat", 5, "How many times should a message be printed to the chat on run start?");
		}
		public override void Hooks()
		{
			CharacterBody.onBodyStartGlobal += LetThereBeFlight;
		}
		private void LetThereBeFlight(CharacterBody characterBody)
        {
			Timer timer = new Timer(2000);
			if (NetworkServer.active && ArtifactEnabled)
			{
				timer.Elapsed += FireFlight;
				timer.AutoReset = true;
				timer.Start();

			} else
            {
				timer.Close();
				timer.Stop();
            }
			void FireFlight(System.Object source, ElapsedEventArgs e)
            {
				if (characterBody.isActiveAndEnabled)
				{
					characterBody.equipmentSlot.FireJetpack();
				}
			}

			
		}
	}
}