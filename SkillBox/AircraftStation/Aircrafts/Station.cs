﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AircraftStation
{
	public class Station : IStation
	{
		public EventHandler OnStationStateChange { get; set; }

		private readonly int maxAircrafts;
		
		private readonly Stack<Aircraft> aircraftsStack;

		public Station(int maxAircrafts)
		{
			this.maxAircrafts = maxAircrafts;
			aircraftsStack = new Stack<Aircraft>();
		}

		public bool TryTakeAircraft(string id)
		{
			if (!CheckFreeSlots(out _))
				return false;
			if (!aircraftsStack.Select(o => o.Id).Contains(id))
			{
				var aircraft = new Aircraft(id);
				aircraftsStack.Push(aircraft);
				OnStationStateChange?.Invoke(this, EventArgs.Empty);
			}

			return true;
		}

		public bool TrySendAircraft(out string aircraftId)
		{
			aircraftId = string.Empty;
			if (!aircraftsStack.Any())
				return false;
			var nextAircraft = aircraftsStack.Pop();
			aircraftId = nextAircraft.Id;
			OnStationStateChange?.Invoke(this, EventArgs.Empty);
			return true;
		}

		public bool CheckFreeSlots(out int freeSlots)
		{
			freeSlots = maxAircrafts - aircraftsStack.Count;
			return freeSlots > 0;
		}

		public int GetAircraftsCountOnStation() => aircraftsStack.Count;

		public bool AnyAircraftsOnStation() => aircraftsStack.Any();

		public bool CheckOnAircraftOnStation(string id) => aircraftsStack.Any(a => a.Id == id);
	}
}