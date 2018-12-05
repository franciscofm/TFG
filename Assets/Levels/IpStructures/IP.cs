using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IP {

	public static IP Empty = new IP ("0.0.0.0");

	public uint numeric;
	public uint[] array;
	public string word;
	public uint mask; //TODO

	public enum Class { A, B, C, D, E };
	public Class class_;

	public IP(uint[] array) {
		if (array.Length != 4) throw new Exception ("IP length not 4");
		if (array.ArrayAboveUInt (255)) throw new Exception ("IP values out of range");

		this.array = array;
		word = ArrayToWord (array);
		numeric = ArrayToUInt (array);

		if 		(array [0] < 128) class_ = Class.A;
		else if (array [0] < 192) class_ = Class.B;
		else if (array [0] < 224) class_ = Class.C;
		else if (array [0] < 240) class_ = Class.D;
		else 					  class_ = Class.E;

	}
	public IP(string ip) : this(ip.Split (new string[]{ "." }, 0x0).StringToUInt ()) {

	}

	public bool IsSubnet(IP net, uint mask) {
		return (numeric & this.mask) == (net.numeric & mask);
	}

	string ArrayToWord(uint[] array) {
		if (array == null || array.Length < 1)
			return "";
		string ret = array [0] + "";
		for (int i = 1; i < array.Length; ++i)
			ret += "." + array [i];
		return ret;
	}
	uint ArrayToUInt(uint[] array) {
		uint number = array [0];
		for (int i = 1; i < array.Length; ++i) {
			number = number << 4;
			number |= array [i];
		}
		return number;
	}

	public override string ToString () {
		return word + ", " + numeric;	
	}
	public bool Equals(IP other) {
		return numeric == other.numeric;
	}
	public override int GetHashCode () {
		return numeric.GetHashCode ();
	}
}
