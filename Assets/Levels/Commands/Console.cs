using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Console {

	public static string jump = Environment.NewLine;

	delegate void Template(string[] command, Shell shell, CommandStructure value);

	private static readonly Dictionary<string, Template> commands = new Dictionary<string, Template>() {
		{"help", help },
		{"man", man },
		{"theme", theme },
		{"history", history },

		{"ifconfig", ifconfig },
		{"ic", ifconfig },
		{"ifup", ifup },
		{"ifdown", ifdown },
		{"route", route },
		{"ping", ping },


		{"ls", ls },
		{"mkdir", mkdir },
		{"mkfile", mkfile },
		{"cd", cd }
	};
	private static string[] AvailableCommands = new string[] {
		"help", 
		"man", 
		"theme",
		"history",

		"ifconfig", 
		"ic", 
		"ifup", 
		"ifdown", 
		"route", 
		"ping", 


		"ls", 
		"mkdir", 
		"mkfile", 
		"cd"
	};

	public static CommandStructure ReadCommand(string[] command, Shell shell) {
		CommandStructure commandReturn = new CommandStructure();
		if (commands.ContainsKey (command [0]) && AvailableCommands.Has(command[0])) {
			commands [command [0]] (command.SubArray (1, command.Length - 1), shell, commandReturn);
		}
		return commandReturn;
	}
	static void help(string[] command, Shell shell, CommandStructure value) {
		value.value = "List of avaliable commands:"+jump;
		foreach (string key in AvailableCommands) 
			value.value += key + jump;
		value.correct = true;
		value.prompt = true;
	}
	static void man(string[] command, Shell shell, CommandStructure value) {
		//Ls.Command (command, shell, value);
	}
	static void theme(string[] command, Shell shell, CommandStructure value) {
		Theme.Command (command, shell, value);
	}
	static void history(string[] command, Shell shell, CommandStructure value) {
		History.Command (command, shell, value);
	}

	//IT
	static void ifconfig(string[] command, Shell shell, CommandStructure value) {
		Ifconfig.Command (command, shell, value);
	}
	static void ifup(string[] command, Shell shell, CommandStructure value) {
		Ifconfig.IfUp (command, shell, value);
	}
	static void ifdown(string[] command, Shell shell, CommandStructure value) {
		Ifconfig.IfDown (command, shell, value);
	}

	static void route(string[] command, Shell shell, CommandStructure value) {
		Route.Command (command, shell, value);
	}

	static void ping(string[] command, Shell shell, CommandStructure value) {
		Ping.Command (command, shell, value);
	}

	//File System
	static void ls(string[] command, Shell shell, CommandStructure value) {
		Ls.Command (command, shell, value);
	}
	static void mkdir(string[] command, Shell shell, CommandStructure value) {
		Mkdir.Command (command, shell, value);
	}
	static void mkfile(string[] command, Shell shell, CommandStructure value) {
		//Mkfile.Command (command, shell, value);
	}
	static void cd(string[] command, Shell shell, CommandStructure value) {
		Cd.Command (command, shell, value);
	}

}

public class CommandStructure { 
	public bool correct = false;
	public bool prompt = false;
	public string value = "";

	public CommandStructure() {
		
	}
}