using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager { 

	public static class Sound {

		public static GameObject Source;

		//Volume
		static float VOLUME_MASTER = 1f;
		static float VOLUME_MUSIC = 1f;
		static float VOLUME_EFFECTS = 1f;
		static float VOLUME_VOICE = 1f;
		static float MAX_MASTER, MAX_MUSIC, MAX_EFFECTS, MAX_VOICE;

		public static void SetAllVolumes(float master, float music, float effects, float voice) {
			VOLUME_MASTER = ClampedVolume(master * MAX_MASTER);
			VOLUME_MUSIC = ClampedVolume(music * MAX_MUSIC);
			VOLUME_VOICE = ClampedVolume(effects * MAX_VOICE);
			VOLUME_EFFECTS = ClampedVolume(voice * MAX_EFFECTS);
			UpdateMasterVolume ();
		}
		public static void SetMasterVolume(float volume) {
			VOLUME_MASTER = ClampedVolume(volume * MAX_MASTER);
			UpdateMasterVolume ();
		}
		public static void SetMusicVolume(float volume) {
			VOLUME_MUSIC = ClampedVolume(volume * MAX_MUSIC);
			UpdateTypeVolume (BackgroundsL,VOLUME_MUSIC);
		}
		public static void SetVoiceVolume(float volume) {
			VOLUME_VOICE = ClampedVolume(volume * MAX_VOICE);
	        UpdateTypeVolume(NarrationsL,VOLUME_VOICE);
		}
		public static void SetEffectVolume(float volume) {
			VOLUME_EFFECTS = ClampedVolume(volume * MAX_EFFECTS);
	        UpdateTypeVolume(EffectsL,VOLUME_EFFECTS);
		}

		static float ClampedVolume(float volume) {
			if (volume < 0f) return 0f;
			if (volume <= 1f) return volume;
			if (volume < 100f) return volume / 100f;
			return 1f;
		}
		static void UpdateMasterVolume() {
			UpdateTypeVolume (BackgroundsL,VOLUME_MUSIC);
			UpdateTypeVolume (EffectsL,VOLUME_EFFECTS);
			UpdateTypeVolume (NarrationsL,VOLUME_VOICE);
		}
		static void UpdateTypeVolume(List<SoundSource> list, float volume) {
			if(list != null)
				for (int i = 0; i < list.Count; ++i)
					list [i].source.volume = volume * VOLUME_MASTER;
		}

		static bool MUTED;
		public static bool Mute() {
			MUTED = !MUTED;
			SetMusicVolume (MUTED ? 0f : 1f);
			return MUTED;
		}

		//Availability
		//public static int USED_SOURCES { get ; private set; }
		public const int MAX_SOURCES = 32;
		public const int MAX_CHANNELS = 12;

		//Channel position
		public const int CH_BACKGROUND = 0;
		public const int CH_NARRATION = 1;
		public const int CH_EFFECT = 2;
		public static int CH_FIRST_FREE { get ; private set; }

		//Channel queue flags
		public const int FLAG_PRIORITY_LOW = 0x01;  //001
		public const int FLAG_PRIORITY_HIGH = 0x02; //010
		public const int FLAG_CLEAR_CHANNEL = 0x04; //100

		//Clip flags
		public const int TYPE_BACKGROUND = 0x01;
		public const int TYPE_EFFECT = 0x02;
		public const int TYPE_NARRATION = 0x04;
		//0x08, 0x10, 0x20, 0x40

	//	public static AudioClip[] Narrations;
		static AudioClip[] Effects;

		static List<SoundSource> EffectsL;
		static List<SoundSource> NarrationsL;
		static List<SoundSource> BackgroundsL;

		static List<SoundSource>[] Channels;

		public static void Init(GameObject sourceModel, float backgroundMax = 1f, float effectMax = 1f, float narrationMax = 1f, float globalMax = 1f) {
			Source = sourceModel;

			//Los maximos capados
			MAX_MUSIC = backgroundMax;
			MAX_EFFECTS = effectMax;
			MAX_VOICE = narrationMax;
			MAX_MASTER = globalMax;

			//Volumen actual multiplicado por los maximos
			SetMasterVolume (1f);
			SetEffectVolume (1f);
			SetVoiceVolume (1f);
			SetMusicVolume (1f);

			CH_FIRST_FREE = 1;
			Channels = new List<SoundSource>[MAX_CHANNELS];
			for (int i = 0; i < MAX_CHANNELS; ++i)
				Channels [i] = new List<SoundSource> ();
			EffectsL = new List<SoundSource> ();
			NarrationsL = new List<SoundSource> ();
			BackgroundsL = new List<SoundSource> ();

		}


		public static SoundSource PlayNarration(AudioClip narration, Vector3 position) {
			return PlaySound (narration, position, false, 2, FLAG_PRIORITY_LOW, TYPE_NARRATION);
		}
		public static SoundSource PlayEffect(AudioClip effect, Vector3 position) {
			return PlaySound (effect, position, false, 1, FLAG_PRIORITY_HIGH, TYPE_EFFECT);
		}
		public static SoundSource PlayBackground(AudioClip background, Vector3 position) {
			return PlaySound (background, position, true, CH_BACKGROUND, FLAG_CLEAR_CHANNEL, TYPE_BACKGROUND);
		}
		public static SoundSource PlaySound(AudioClip clip, Vector3 position, bool looped = false, int channel = 1, int priority = FLAG_PRIORITY_LOW, int type = TYPE_EFFECT) {

			if (clip == null) return Error ("clip is NULL.");
			if (channel < 0 || MAX_CHANNELS <= channel) return Error ("channel is (<0) or (>MAX_CHANNELS).");

			//Source creation
			GameObject source = GameObject.Instantiate (Source);
			GameObject.DontDestroyOnLoad (source);
			source.transform.position = position;
			SoundSource mss = source.GetComponent<SoundSource>();

			//Type and list reference
			List<SoundSource> reference = null;
			float volume = VOLUME_MASTER;
			switch (type) {
				case TYPE_BACKGROUND:
					reference = BackgroundsL;
					volume *= VOLUME_MUSIC;
					break;
				case TYPE_NARRATION:
					reference = NarrationsL;
					volume *= VOLUME_VOICE;
					break;
				default:
					reference = EffectsL;
					volume *= VOLUME_EFFECTS;
					break;
			}

			mss.Init (clip, looped, channel, reference);
			mss.source.volume = volume;
			reference.Add (mss);

			//Queue placement
			//Clear & priority
			if ((priority & FLAG_CLEAR_CHANNEL) == FLAG_CLEAR_CHANNEL) {
				StopChannel (channel);
				Channels [channel].Add (mss);
				mss.Play ();
			} else {
				if ((priority & FLAG_PRIORITY_LOW) == FLAG_PRIORITY_LOW) {
					Channels [channel].Add (mss);
					if (Channels [channel].Count == 1)
						mss.Play ();
				}else if ((priority & FLAG_PRIORITY_HIGH) == FLAG_PRIORITY_HIGH) {
					Channels [channel].Insert (0, mss);
					mss.Play ();
				}
			}
			if(CH_FIRST_FREE == channel) FindFree ();

			return mss;
		}

		public static void HitNext(int channel) {
			if (0 < Channels [channel].Count) {
				if (Channels [channel] [0].source.isPlaying || Channels [channel] [0].paused)
					return;
				else {
					Channels [channel] [0].Play ();
					return;
				}
			}
		}

		public static void StopSound(SoundSource mss) {
			Channels [mss.channel].Remove (mss);
			mss.reference.Remove (mss);
			HitNext (mss.channel);
			GameObject.Destroy (mss.gameObject);
		}
		public static void StopChannel(int channel, float duration = 0f) {
			List<SoundSource> list = Channels [channel];
			while (list.Count > 0)
				StopSound (list [0]);
		}
		public static void StopType(int type = TYPE_NARRATION, float duration = 0f) {
			List<SoundSource> list = null;
			switch (type) {
			case TYPE_EFFECT:
				list = EffectsL;
				break;
			case TYPE_BACKGROUND:
				list = BackgroundsL;
				break;
			case TYPE_NARRATION:
				list = NarrationsL;
				break;
			}
			while (list.Count > 0) 
				StopSound (list [0]);
		}
		public static void StopAll(float duration = 0f) {
			for (int i = 0; i < MAX_CHANNELS; ++i)
				StopChannel (i,duration);
		}

		static void FindFree() {
			for (int i = 1; i < Channels.Length; ++i)
				if (Channels [i].Count == 0) {
					CH_FIRST_FREE = i;
					return;
				}
			CH_FIRST_FREE = 1;
		}

	    static SoundSource Error(string msg) {
			Debug.Log ("ManagerSound error: " + msg);
			return null;
		}

	}

}