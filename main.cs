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
    static string filePath = "channels.txt"; // File path to save the channel list

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

            if (choice == "1")
            {
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
            }
            else if (choice == "2")
            {
                Console.WriteLine("Saved channels:");
                ShowChannelList();
            }
            else if (choice == "3")
            {
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
            }
            else if (choice == "4")
            {
                Console.WriteLine("Enter the channel number to open:");
                int channelNumber;
                if (int.TryParse(Console.ReadLine(), out channelNumber))
                {
                    OpenChannelURL(channelNumber);
                }
                else
                {
                    Console.WriteLine("Invalid channel number!");
                }
            }
            else if (choice == "5")
            {
                Console.WriteLine("Enter the channel number to edit:");
                int channelNumber;
                if (int.TryParse(Console.ReadLine(), out channelNumber))
                {
                    EditChannel(channelNumber);
                    SaveChannelsToFile(); // Save the channels to file
                }
                else
                {
                    Console.WriteLine("Invalid channel number!");
                }
            }
            else if (choice == "0")
            {
                break; // Exit the program
            }
            else
            {
                Console.WriteLine("Invalid choice!");
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
            Console.WriteLine($"Are you sure you want to remove the channel? (Y/N) - {channelToRemove.Name}");
            string confirm = Console.ReadLine();

            if (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                channelList.Remove(channelToRemove);
                return true;
            }
        }
        return false;
    }

    static void EditChannel(int channelNumber)
    {
        Channel channelToEdit = channelList.FirstOrDefault(c => c.Number == channelNumber);
        if (channelToEdit != null)
        {
            Console.WriteLine($"Editing Channel - {channelToEdit.Name}");
            Console.WriteLine("Enter new channel name (leave empty to keep current name):");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                channelToEdit.Name = newName;
            }

            Console.WriteLine("Enter new channel URL (leave empty to keep current URL):");
            string newUrl = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUrl))
            {
                channelToEdit.Url = newUrl;
            }

            Console.WriteLine("Enter new description for the channel (leave empty to keep current description):");
            string newDescription = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDescription))
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

    static void ShowChannelList()
    {
        Console.WriteLine("Channel List");
        Console.WriteLine("---------------------------------------");

        foreach (Channel channel in channelList)
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"Channel Number: {channel.Number}");
            Console.WriteLine($"Name: {channel.Name}");
            Console.WriteLine($"URL: {channel.Url}");
            Console.WriteLine($"Description: {channel.Description}");
        }

        Console.WriteLine("---------------------------------------");
    }

    static void OpenChannelURL(int channelNumber)
    {
        Channel channelToOpen = channelList.FirstOrDefault(c => c.Number == channelNumber);
        if (channelToOpen != null)
        {
            Console.WriteLine($"Are you sure you want to open the channel? (Y/N) - {channelToOpen.Name}");
            string confirm = Console.ReadLine();

            if (confirm.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Process.Start(channelToOpen.Url);
            }
        }
        else
        {
            Console.WriteLine("Channel not found!");
        }
    }

    static void SaveChannelsToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Channel channel in channelList)
            {
                writer.WriteLine($"{channel.Number},{channel.Name},{channel.Url},{channel.Description}");
            }
        }
    }

    static void LoadChannelsFromFile()
    {
        if (File.Exists(filePath))
        {
            channelList.Clear();

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                if (parts.Length == 4 && int.TryParse(parts[0], out int channelNumber))
                {
                    Channel channel = new Channel
                    {
                        Number = channelNumber,
                        Name = parts[1],
                        Url = parts[2],
                        Description = parts[3]
                    };

                    channelList.Add(channel);
                }
            }
        }
    }

    static int GetNextChannelNumber()
    {
        return channelList.Count + 1;
    }
}
