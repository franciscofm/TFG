using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Console {

	public static string jump = Environment.NewLine;

	delegate void Template(string[] command, Shell shell, CommandStructure value);

	private static readonly Dictionary<string, Template> commands = new Dictionary<string, Template>() {
		{"help", help },

		{"ifconfig", ifconfig },
		{"ifup", ifup },
		{"ifdown", ifdown },

		{"theme", theme },

		{"route", route },

		{"ping", ping }
	};

	public static CommandStructure ReadCommand(string[] command, Shell shell) {
		CommandStructure commandReturn = new CommandStructure();
		if (commands.ContainsKey (command [1])) {
			commands [command [1]] (command.SubArray (2, command.Length - 2), shell, commandReturn);
		}
		return commandReturn;
	}
	static void help(string[] command, Shell shell, CommandStructure value) {
		value.value = "List of avaliable commands:"+jump;
		foreach (string key in commands.Keys) 
			value.value += key + jump;
		value.correct = true;
		value.prompt = true;
	}

	//ifconfig
	static void ifconfig(string[] command, Shell shell, CommandStructure value) {
		Ifconfig.Command (command, shell, value);
	}
	static void ifup(string[] command, Shell shell, CommandStructure value) {
		Ifconfig.IfUp (command, shell, value);
	}
	static void ifdown(string[] command, Shell shell, CommandStructure value) {
		Ifconfig.IfDown (command, shell, value);
	}

	static void ping(string[] command, Shell shell, CommandStructure value) {
		Ping.Command (command, shell);
	}
	static void route(string[] command, Shell shell, CommandStructure value) {
		Route.Command (command, shell, value);
	}
	
	static void theme(string[] command, Shell shell, CommandStructure value) {
		Theme.Command (command, shell, value);
	}
}

public class CommandStructure { 
	public bool correct = false;
	public bool prompt = false;
	public string value = "";

	public CommandStructure() {
		
	}
}