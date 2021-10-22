using System;
using System.Reflection;
//using static HarmonyLib.Harmony;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace OxygenNotIncluded.Mods.buildingNotifications
{
	public static class Loader
	{
		public static AssemblyName AssemblyName => Assembly.GetExecutingAssembly().GetName();
		public static Version Version => AssemblyName.Version;
		public static string Name => AssemblyName.Name;


		public static void OnLoad()
		{

			// Called before any other mod functions (including patches), when Mod is loaded by the Game
			Console.WriteLine($"Mod <{Name}> loaded: {Version}");
		}

		static bool loaded = false;
		[HarmonyPatch(typeof(Constructable), "OnCompleteWork")]
		public static class Constructable_OnCompleteWork_Patch
		{
			internal static void Prefix(Constructable __instance, Worker worker)
			{
				loaded = true;
			}
		}

		[HarmonyPatch(typeof(LoadScreen), "OnActivate")]
		public static class LoadScreenOnActivate
		{
			internal static void Prefix()
			{
				loaded = false;
                Console.WriteLine("LoadScreen.OnActivate");
			}
		}
		[HarmonyPatch(typeof(MainMenu), "OnActivate")]
		public static class MainMenuOnActivate
		{
			internal static void Prefix()
			{
				loaded = false;
				Console.WriteLine("MainMenu.OnActivate");
			}
		}
		static string[] whitelist = new string[]
		{
			"BottleEmptier",
			"StorageLocker",
			"CreatureDeliveryPoint",
			"LiquidValve",
			"GasFilter",
			"LiquidFilter",
			"LogicPressureSensorGas",
			"LogicPressureSensorLiquid",
			"SolidConduitInbox",
			"HighEnergyParticleSpawner",
			"HighEnergyParticleRedirector",

		};

		static Queue<Message> last5 = new Queue<Message>();

		[HarmonyPatch(typeof(Building), "OnSpawn")]
        public static class BuildingOnSpawn
		{
            internal static void Postfix(Building __instance)
            {
                if (loaded)
                {

					string name = __instance.ToString();
					if (name.Contains("Preview") || name.Contains("UnderConstruction")) return;
					name = name.Split(' ')[0].Replace("Complete", "");

					if (whitelist.Any(name.StartsWith))
					{
						string msg = "Finished " + name.Replace("HighEnergyParticle", "Radbolt").Replace("Logic", "");
						var notification = new Notification(msg, NotificationType.Good, click_focus: __instance.transform);
						__instance.gameObject.AddOrGet<Notifier>().Add(notification);
					}



					//string name = __instance.ToString();
					//if (name.Contains("Preview")|| name.Contains("UnderConstruction")) return;
					//name = name.Split(' ')[0].Replace("Complete", "");

     //               if (whitelist.Any(name.StartsWith))
     //               {
					//	//File.AppendAllText("buildings.txt", "\"" + name + "\",\n");
					//	string msg = "finished " + name.Replace("HighEnergyParticle", "Radbolt").Replace("Logic", "");
					//	Console.WriteLine(msg);

					//	Message i = new GenericMessage(msg, msg, msg);
					//	Messenger.Instance.QueueMessage(i);
					//	last5.Enqueue(i);
					//	Console.WriteLine("last5.Count " + last5.Count);
					//	if (last5.Count > 5)
     //                   {
					//		Message toRemove = last5.Dequeue();
					//		Messenger.Instance.DequeueMessage();
					//	}
					//}



				}

			}
        }


	}
}