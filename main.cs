using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Channel
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
}

class Program
{
    static List<Channel> channelList = new List<Channel>(); // Channel list
    static string filePath = "channels.csv"; // File path to save the channel list

    static void Main(string[] args)
    {
        LoadChannelsFromFile(); // Load previously saved channels

        while (true)
        {
            Console.WriteLine("Press '1' to add a channel\n" +
                              "Press '2' to view the channel list\n" +
                              "Press '3' to remove a channel\n" +
                              "Press '4' to open a channel URL\n" +
                              "Press '5' to edit a channel\n" +
                              "Press '0' to exit the program.");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter the channel name:");
                    string name = Console.ReadLine();

                    Console.WriteLine("Enter the channel URL:");
                    string url = Console.ReadLine();

                    Console.WriteLine("Enter a description for the channel:");
                    string description = Console.ReadLine();

                    Channel channel = new Channel
                    {
                        Number = GetNextChannelNumber(),
                        Name = name,
                        Url = url,
                        Description = description
                    };

                    AddChannel(channel);
                    Console.WriteLine("Channel added successfully!");

                    SaveChannelsToFile(); // Save the channels to file
                    break;

                case "2":
                    Console.WriteLine("Saved channels:");
                    ShowChannelList();
                    break;

                case "3":
                    Console.WriteLine("Enter the channel number to remove:");
                    int channelNumber;
                    if (int.TryParse(Console.ReadLine(), out channelNumber))
                    {
                        if (RemoveChannel(channelNumber))
                        {
                            Console.WriteLine("Channel removed successfully!");
                            SaveChannelsToFile(); // Save the channels to file
                        }
                        else
                        {
                            Console.WriteLine("Channel not found!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid channel number!");
                    }
                    break;

                case "4":
                    Console.WriteLine("Enter the channel number to open:");
                    int channelNumberToOpen;
                    if (int.TryParse(Console.ReadLine(), out channelNumberToOpen))
                    {
                        OpenChannelURL(channelNumberToOpen);
                    }
                    else
                    {
                        Console.WriteLine("Invalid channel number!");
                    }
                    break;

                case "5":
                    Console.WriteLine("Enter the channel number to edit:");
                    int channelNumberToEdit;
                    if (int.TryParse(Console.ReadLine(), out channelNumberToEdit))
                    {
                        EditChannel(channelNumberToEdit);
                        SaveChannelsToFile(); // Save the channels to file
                    }
                    else
                    {
                        Console.WriteLine("Invalid channel number!");
                    }
                    break;

                case "0":
                    return; // Exit the program

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void AddChannel(Channel channel)
    {
        channelList.Add(channel);
    }

    static bool RemoveChannel(int channelNumber)
    {
        Channel channelToRemove = channelList.FirstOrDefault(c => c.Number == channelNumber);
        if (channelToRemove != null)
        {
            Console.WriteLine($"Are you sure you want to remove Channel {channelToRemove.Name}? (Y/N)");
            string confirmation = Console.ReadLine();
            if (confirmation.ToUpper() == "Y")
            {
                channelList.Remove(channelToRemove);
                return true;
            }
        }
        return false;
    }

    static void ShowChannelList()
    {
        foreach (Channel channel in channelList)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine($"Channel Number: {channel.Number}");
            Console.WriteLine($"Name: {channel.Name}");
            Console.WriteLine($"URL: {channel.Url}");
            Console.WriteLine($"Description: {channel.Description}");
            Console.WriteLine("---------------------------");
        }
    }

    static void OpenChannelURL(int channelNumber)
    {
        Channel channelToOpen = channelList.FirstOrDefault(c => c.Number == channelNumber);
        if (channelToOpen != null)
        {
            Process.Start(channelToOpen.Url);
        }
        else
        {
            Console.WriteLine("Channel not found!");
        }
    }

    static void EditChannel(int channelNumber)
{
    Channel channelToEdit = channelList.FirstOrDefault(c => c.Number == channelNumber);
    if (channelToEdit != null)
    {
        Console.WriteLine($"Editing Channel {channelToEdit.Name}:");
        Console.WriteLine("Enter the new channel name (leave empty to keep the same):");
        string newName = Console.ReadLine();

        Console.WriteLine("Enter the new channel URL (leave empty to keep the same):");
        string newUrl = Console.ReadLine();

        Console.WriteLine("Enter the new description for the channel (leave empty to keep the same):");
        string newDescription = Console.ReadLine();

        if (!string.IsNullOrEmpty(newName))
        {
            channelToEdit.Name = newName;
        }

        if (!string.IsNullOrEmpty(newUrl))
        {
            channelToEdit.Url = newUrl;
        }

        if (!string.IsNullOrEmpty(newDescription))
        {
            channelToEdit.Description = newDescription;
        }

        Console.WriteLine("Channel edited successfully!");
    }
    else
    {
        Console.WriteLine("Channel not found!");
    }
}


    static int GetNextChannelNumber()
    {
        if (channelList.Count > 0)
        {
            return channelList.Max(c => c.Number) + 1;
        }
        else
        {
            return 1;
        }
    }

    static void LoadChannelsFromFile()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        int number = int.Parse(parts[0]);
                        string name = parts[1];
                        string url = parts[2];
                        string description = parts[3];

                        Channel channel = new Channel
                        {
                            Number = number,
                            Name = name,
                            Url = url,
                            Description = description
                        };

                        channelList.Add(channel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while loading channels: {ex.Message}");
            }
        }
    }

    static void SaveChannelsToFile()
    {
        try
        {
            List<string> lines = new List<string>();
            foreach (Channel channel in channelList)
            {
                string line = $"{channel.Number},{channel.Name},{channel.Url},{channel.Description}";
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while saving channels: {ex.Message}");
        }
    }
}
