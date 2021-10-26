using System;
using System.Reflection;
//using static HarmonyLib.Harmony;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace OxygenNotIncluded.Mods.noUselessNotifications
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



		[HarmonyPatch(typeof(Notifier))]
		[HarmonyPatch("Add")]
		public class Notifier_patch
		{
			public static bool Prefix(Notification notification, string suffix)
			{
				//if()
				////Cycle 122 report ready
				//Console.WriteLine("Notifier_patch Notifier_patch Notifier_patch Notifier_patch");
				//            Console.WriteLine(notification.titleText);
				//            Console.WriteLine(notification);
				//            Console.WriteLine(suffix);
				return !(notification.titleText.Contains("report ready") || notification.titleText.Contains("Attribute increase"));
			}
		}


	}
}