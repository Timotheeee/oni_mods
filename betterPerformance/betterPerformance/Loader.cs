using System;
using System.Reflection;
//using static HarmonyLib.Harmony;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using STRINGS;
using Klei.AI;
using TUNING;
using static EdiblesManager;

namespace OxygenNotIncluded.Mods.betterPerformance
{
	public static class Loader
	{
		public static AssemblyName AssemblyName => Assembly.GetExecutingAssembly().GetName();
		public static Version Version => AssemblyName.Version;
		public static string Name => AssemblyName.Name;


		[HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier")]
		private class GetEfficiencyMultiplier_Patch
		{
			public static void Postfix(Worker worker, ref Workable __instance, ref float __result)
			{
                //Console.WriteLine("betterPerformance postfix GetEfficiencyMultiplier");
				if (!(__instance is Edible))
				{
					__result *= 2;
				}
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public static class Db_Initilaize_Patch
		{
			internal static void Postfix(Db __instance)
			{
				float athletics = 2;
				float basespeed = 2;

				//Debug.Log("-->  betterPerformance Db Postfix begin.");
				//Debug.Log($"db = {__instance}");
				if (__instance != null)
				{
					Database.AttributeConverters attributeConverters = __instance.AttributeConverters;
					//Debug.Log($"converters = {attributeConverters}");
					ToPercentAttributeFormatter toPercentAttributeFormatter = new ToPercentAttributeFormatter(1f);
					if (attributeConverters != null)
					{
						AttributeConverter attributeConverter = attributeConverters.Create("MovementSpeed", "Movement Speed", (string)DUPLICANTS.ATTRIBUTES.ATHLETICS.SPEEDMODIFIER, Db.Get().Attributes.Athletics, athletics, basespeed, (IAttributeFormatter)toPercentAttributeFormatter, DlcManager.AVAILABLE_ALL_VERSIONS);
						//Debug.Log($"ac = {attributeConverter}");
						attributeConverters.MovementSpeed = attributeConverter;
					}
				}
				//Debug.Log("-->  betterPerformance Db Postfix end.");
			}
		}



        [HarmonyPatch(typeof(Db), "Initialize")]
		public class RocketDbInitialize
		{
			private static void Prefix()
			{
				ROCKETRY.ENGINE_POWER.EARLY_WEAK *= 2;
				ROCKETRY.ENGINE_POWER.EARLY_STRONG *= 2;
				ROCKETRY.ENGINE_POWER.MID_VERY_STRONG *= 2;
				ROCKETRY.ENGINE_POWER.MID_STRONG *= 2;
				ROCKETRY.ENGINE_POWER.LATE_STRONG *= 2;
				ROCKETRY.ENGINE_POWER.LATE_VERY_STRONG *= 2;

			}
		}


		[HarmonyPatch(typeof(HighEnergyParticleSpawner), "LauncherUpdate")]
		public static class afasfsa
		{
			internal static bool Prefix(ref float dt)
			{
				dt *= 2;
				return true;
			}
		}

		[HarmonyPatch(typeof(MinionConfig), "AddMinionTraits")]
		public static class afasaafsa
		{
			internal static void Postfix(string name, Modifiers modifiers)
			{
				Trait trait = Db.Get().CreateTrait("extrahunger", name, name, null, should_save: false, null, positive_trait: true, is_valid_starter_trait: true);
				trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -833.333f, name));
				trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 2000000f, name));

				modifiers.initialTraits.Add("extrahunger");
			}
		}





	}
}