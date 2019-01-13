using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Console {

	public static string jump = Environment.NewLine;

	public delegate void ConsoleEvent(CommandStructure com);
	public static event ConsoleEvent OnCommandRead;

	delegate void ShellTemplate(string[] command, Shell shell, CommandStructure value);
	delegate void NodeTemplate(string[] command, Node node, CommandStructure value);

	private static readonly Dictionary<string, ShellTemplate> shellCommands = new Dictionary<string, ShellTemplate>() {
		{"help", help },
		{"man", man },
		{"theme", theme },
		{"history", history },

		{"ls", ls },
		{"mkdir", mkdir },
		{"mkfile", mkfile },
		{"cd", cd }
	};
	private static readonly Dictionary<string, NodeTemplate> nodeCommands = new Dictionary<string, NodeTemplate>() {
		{"ifconfig", ifconfig },
		{"ic", ifconfig },
		{"ifup", ifup },
		{"ifdown", ifdown },
		{"route", route },
		{"ping", ping }
	};

	private static string[] AvailableCommands = new string[] {
		"help", "man", "theme", "history",
		"ifconfig", "ic", "ifup", "ifdown", "route", "ping", 
		"ls", "mkdir", "mkfile", "cd"
	};

	public static CommandStructure ReadCommand(string[] command, Shell shell) {
		CommandStructure commandReturn = new CommandStructure();
		commandReturn.command = command;
		commandReturn.shell = shell;
		if (shellCommands.ContainsKey (command [0]) && AvailableCommands.Has(command[0])) {
			shellCommands [command [0]] (command.SubArray (1, command.Length - 1), shell, commandReturn);
		} else if(nodeCommands.ContainsKey (command [0]) && AvailableCommands.Has(command[0])) {
			nodeCommands [command [0]] (command.SubArray (1, command.Length - 1), shell.node, commandReturn);
		}
		if (OnCommandRead != null) OnCommandRead (commandReturn);
		return commandReturn;
	}
	public static CommandStructure ReadCommand(string[] command, Node node) {
		CommandStructure commandReturn = new CommandStructure();
		commandReturn.command = command;
		commandReturn.node = node;
		if(nodeCommands.ContainsKey (command [0]) && AvailableCommands.Has(command[0])) {
			nodeCommands [command [0]] (command.SubArray (1, command.Length - 1), node, commandReturn);
		}
		if (OnCommandRead != null) OnCommandRead (commandReturn);
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
		//Man.Command (command, shell, value);
	}
	static void theme(string[] command, Shell shell, CommandStructure value) {
		Theme.Command (command, shell, value);
	}
	static void history(string[] command, Shell shell, CommandStructure value) {
		History.Command (command, shell, value);
	}

	//IT
	static void ifconfig(string[] command, Node node, CommandStructure value) {
		Ifconfig.Command (command, node, value);
	}
	static void ifup(string[] command, Node node, CommandStructure value) {
		Ifconfig.IfUp (command, node, value);
	}
	static void ifdown(string[] command, Node node, CommandStructure value) {
		Ifconfig.IfDown (command, node, value);
	}

	static void route(string[] command, Node node, CommandStructure value) {
		Route.Command (command, node, value);
	}

	static void ping(string[] command, Node node, CommandStructure value) {
		Ping.Command (command, node, value);
	}

	//File System
	static void ls(string[] command, Shell shell, CommandStructure value) {
		Ls.Command (command, shell, value);
	}
	static void mkdir(string[] command, Shell shell, CommandStructure value) {
		Mkdir.Command (command, shell, value);
	}
	static void mkfile(string[] command, Shell shell, CommandStructure value) {
		Mkfile.Command (command, shell, value);
	}
	static void cd(string[] command, Shell shell, CommandStructure value) {
		Cd.Command (command, shell, value);
	}

}

public class CommandStructure {
	public string[] command;
	public Shell shell;
	public Node node;
	public bool correct = false;
	public bool prompt = false;
	public string value = "";

	public CommandStructure() {
		
	}
}