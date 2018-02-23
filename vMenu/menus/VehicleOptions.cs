﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NativeUI;

namespace vMenuClient
{
    public class VehicleOptions
    {
        #region Variables
        // Menu variable, will be defined in CreateMenu()
        private UIMenu menu;
        private Notification Notify = MainMenu.Notify;
        private Subtitles Subtitle = MainMenu.Subtitle;
        private CommonFunctions cf = MainMenu.cf;
        private static VehicleData vd = new VehicleData();

        // Submenus
        public UIMenu vehicleModMenu { get; private set; }
        public UIMenu vehicleDoorsMenu { get; private set; }
        public UIMenu vehicleWindowsMenu { get; private set; }
        public UIMenu vehicleComponents { get; private set; }
        public UIMenu vehicleLiveries { get; private set; }
        public UIMenu vehicleColors { get; private set; }
        public UIMenu deleteConfirm { get; private set; }

        // Public variables (getters only), return the private variables.
        public bool VehicleGodMode { get; private set; } = false;
        public bool VehicleEngineAlwaysOn { get; private set; } = true;
        public bool VehicleNoSiren { get; private set; } = false;
        public bool VehicleNoBikeHelemet { get; private set; } = false;
        public bool VehicleFrozen { get; private set; } = false;
        public bool VehicleTorqueMultiplier { get; private set; } = false;
        public bool VehiclePowerMultiplier { get; private set; } = false;
        public float VehicleTorqueMultiplierAmount { get; private set; } = 2f;
        public float VehiclePowerMultiplierAmount { get; private set; } = 2f;
        #endregion

        #region CreateMenu()
        /// <summary>
        /// Create menu creates the vehicle options menu.
        /// </summary>
        private void CreateMenu()
        {
            // Create the menu.
            menu = new UIMenu(GetPlayerName(PlayerId()), "Vehicle Options")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseEdgeEnabled = false
            };

            #region menu variables
            // Create Checkboxes.
            UIMenuCheckboxItem vehicleGod = new UIMenuCheckboxItem("God Mode", VehicleGodMode, "Disables any type of visual or physical damage to your vehicle.");
            UIMenuCheckboxItem vehicleEngineAO = new UIMenuCheckboxItem("Engine Always On", VehicleEngineAlwaysOn, "Keeps your vehicle engine on when you exit your vehicle.");
            UIMenuCheckboxItem vehicleNoSiren = new UIMenuCheckboxItem("Disable Siren", VehicleNoSiren, "Disables your vehicle's siren. Only works if your vehicle actually has a siren.");
            UIMenuCheckboxItem vehicleNoBikeHelmet = new UIMenuCheckboxItem("No Bike Helmet", VehicleNoBikeHelemet, "No longer auto-equip a helmet when getting on a bike or quad.");
            UIMenuCheckboxItem vehicleFreeze = new UIMenuCheckboxItem("Freeze Vehicle", VehicleFrozen, "Freeze your vehicle's position.");
            UIMenuCheckboxItem torqueEnabled = new UIMenuCheckboxItem("Enable Torque Multiplier", VehicleTorqueMultiplier, "Enables the torque multiplier selected from the list below.");
            UIMenuCheckboxItem powerEnabled = new UIMenuCheckboxItem("Enable Power Multiplier", VehiclePowerMultiplier, "Enables the power multiplier selected from the list below.");

            // Create buttons.
            UIMenuItem fixVehicle = new UIMenuItem("Repair Vehicle", "Repair any visual and physical damage present on your vehicle.");
            UIMenuItem cleanVehicle = new UIMenuItem("Wash Vehicle", "Clean your vehicle.");
            UIMenuItem toggleEngine = new UIMenuItem("Toggle Engine On/Off", "Turn your engine on/off.");
            UIMenuItem setLicensePlateText = new UIMenuItem("Set License Plate Text", "Enter a custom license plate for your vehicle.");
            UIMenuItem modMenuBtn = new UIMenuItem("Mod Menu", "Tune and customize your vehicle here.");
            UIMenuItem doorsMenuBtn = new UIMenuItem("Vehicle Doors", "Open, close, remove and restore vehicle doors here.");
            UIMenuItem windowsMenuBtn = new UIMenuItem("Vehicle Windows", "Roll your windows up/down or remove/restore your vehicle windows here.");
            UIMenuItem componentsMenuBtn = new UIMenuItem("Vehicle Extras", "Add/remove vehicle components/extras.");
            UIMenuItem liveriesMenuBtn = new UIMenuItem("Vehicle Liveries", "Style your vehicle with fancy liveries!");
            UIMenuItem colorsMenuBtn = new UIMenuItem("Vehicle Colors", "Style your vehicle even further by giving it some ~g~Snailsome ~s~colors!");
            UIMenuItem deleteBtn = new UIMenuItem("~r~Delete Vehicle", "Delete your vehicle, this ~r~can NOT be undone~s~!");
            deleteBtn.SetRightBadge(UIMenuItem.BadgeStyle.Alert);
            UIMenuItem deleteNoBtn = new UIMenuItem("NO, CANCEL", "NO, do NOT delete my vehicle and go back!");
            UIMenuItem deleteYesBtn = new UIMenuItem("~r~YES, DELETE", "Yes I'm sure, delete my vehicle please, I understand that this cannot be undone.");
            deleteYesBtn.SetRightBadge(UIMenuItem.BadgeStyle.Alert);
            deleteYesBtn.SetLeftBadge(UIMenuItem.BadgeStyle.Alert);

            UIMenuItem flipVehicle = new UIMenuItem("Flip Vehicle", "Sets your current vehicle on all 4 wheels.");
            UIMenuItem vehicleAlarm = new UIMenuItem("Toggle Vehicle Alarm", "Starts/stops your vehicle's alarm.");
            UIMenuItem cycleSeats = new UIMenuItem("Cycle Through Vehicle Seats", "Cycle through the available vehicle seats.");

            // Create lists.
            var dirtlevel = new List<dynamic> { "Clean", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            UIMenuListItem setDirtLevel = new UIMenuListItem("Set Dirt Level", dirtlevel, 0, "Select how much dirt should be visible on your vehicle. This won't freeze the dirt level, it will only set it once.");
            var licensePlates = new List<dynamic> { GetLabelText("CMOD_PLA_0"), GetLabelText("CMOD_PLA_1"), GetLabelText("CMOD_PLA_2"), GetLabelText("CMOD_PLA_3"), GetLabelText("CMOD_PLA_4"), "North Yankton" };
            UIMenuListItem setLicensePlateType = new UIMenuListItem("License Plate Type", licensePlates, 0, "Select a license plate type and press enter to apply it to your vehicle.");
            var torqueMultiplierList = new List<dynamic> { "x2", "x4", "x8", "x16", "x32", "x64", "x128", "x256", "x512", "x1024" };
            UIMenuListItem torqueMultiplier = new UIMenuListItem("Engine Torque Multiplier", torqueMultiplierList, 0, "Select the engine torque multiplier.");
            var powerMultiplierList = new List<dynamic> { "x2", "x4", "x8", "x16", "x32", "x64", "x128", "x256", "x512", "x1024" };
            UIMenuListItem powerMultiplier = new UIMenuListItem("Engine Power Multiplier", powerMultiplierList, 0, "Select the engine power multiplier.");
            #endregion

            #region Vehicle Options Submenus
            // Submenu's
            vehicleModMenu = new UIMenu("Mod Menu", "Vehicle Mods")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false,
            };
            vehicleDoorsMenu = new UIMenu("Vehicle Doors", "Vehicle Doors Management")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            vehicleWindowsMenu = new UIMenu("Vehicle Windows", "Vehicle Windows Management")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            vehicleComponents = new UIMenu("Vehicle Extras", "Vehicle Extras/Components")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            vehicleLiveries = new UIMenu("Vehicle Liveries", "Vehicle Liveries.")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            vehicleColors = new UIMenu("Vehicle Colors", "Vehicle Colors")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            deleteConfirm = new UIMenu("Confirm Action", "DELETE VEHICLE, ARE YOU SURE?")//, MainMenu.MenuPosition)
            {
                //ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            #endregion

            MainMenu.Mp.Add(vehicleModMenu);
            MainMenu.Mp.Add(vehicleDoorsMenu);
            MainMenu.Mp.Add(vehicleWindowsMenu);
            MainMenu.Mp.Add(vehicleComponents);
            MainMenu.Mp.Add(vehicleLiveries);
            MainMenu.Mp.Add(vehicleColors);
            MainMenu.Mp.Add(deleteConfirm);

            #region Add items to the menu.
            // Add everything to the menu.
            menu.AddItem(vehicleGod); // GOD MODE
            menu.AddItem(fixVehicle); // REPAIR VEHICLE
            menu.AddItem(cleanVehicle); // CLEAN VEHICLE
            menu.AddItem(setDirtLevel); // SET DIRT LEVEL
            menu.AddItem(toggleEngine); // TOGGLE ENGINE ON/OFF
            menu.AddItem(setLicensePlateText); // SET LICENSE PLATE TEXT
            menu.AddItem(setLicensePlateType); // SET LICENSE PLATE TYPE
            menu.AddItem(modMenuBtn); // MOD MENU
            menu.AddItem(colorsMenuBtn); // COLORS MENU
            menu.AddItem(liveriesMenuBtn); // LIVERIES MENU
            menu.AddItem(componentsMenuBtn); // COMPONENTS MENU
            menu.AddItem(doorsMenuBtn); // DOORS MENU
            menu.AddItem(windowsMenuBtn); // WINDOWS MENU
            menu.AddItem(vehicleFreeze); // FREEZE VEHICLE
            menu.AddItem(torqueEnabled); // TORQUE ENABLED
            menu.AddItem(torqueMultiplier); // TORQUE LIST
            menu.AddItem(powerEnabled); // POWER ENABLED
            menu.AddItem(powerMultiplier); // POWER LIST
            menu.AddItem(flipVehicle); // FLIP VEHICLE
            menu.AddItem(vehicleAlarm); // TOGGLE VEHICLE ALARM
            menu.AddItem(cycleSeats); // CYCLE THROUGH VEHICLE SEATS
            menu.AddItem(vehicleEngineAO); // LEAVE ENGINE RUNNING
            menu.AddItem(vehicleNoSiren); // DISABLE SIREN
            menu.AddItem(vehicleNoBikeHelmet); // DISABLE BIKE HELMET
            menu.AddItem(deleteBtn); // DELETE VEHICLE

            #region delete vehicle handle stuff
            deleteConfirm.AddItem(deleteNoBtn);
            deleteConfirm.AddItem(deleteYesBtn);
            deleteConfirm.OnItemSelect += (sender, item, index) =>
            {
                if (item == deleteNoBtn)
                {
                    deleteConfirm.GoBack();
                }
                else
                {
                    var veh = cf.GetVehicle();
                    if (DoesEntityExist(veh) && GetPedInVehicleSeat(veh, -1) == PlayerPedId())
                    {
                        SetEntityAsMissionEntity(veh, false, false);
                        DeleteVehicle(ref veh);
                    }
                    else
                    {
                        Notify.Alert("You need to in the driver's seat if you want to delete a vehicle.");
                    }
                    deleteConfirm.GoBack();
                    menu.GoBack();
                }
            };
            #endregion
            #endregion

            #region Bind Submenus to their buttons.
            menu.BindMenuToItem(vehicleModMenu, modMenuBtn);
            menu.BindMenuToItem(vehicleDoorsMenu, doorsMenuBtn);
            menu.BindMenuToItem(vehicleWindowsMenu, windowsMenuBtn);
            menu.BindMenuToItem(vehicleComponents, componentsMenuBtn);
            menu.BindMenuToItem(vehicleLiveries, liveriesMenuBtn);
            menu.BindMenuToItem(vehicleColors, colorsMenuBtn);
            menu.BindMenuToItem(deleteConfirm, deleteBtn);
            #endregion

            #region Handle button presses
            // Manage button presses.
            menu.OnItemSelect += (sender, item, index) =>
            {
                // If the player is actually in a vehicle, continue.
                if (DoesEntityExist(cf.GetVehicle()))
                {
                    // Create a vehicle object.
                    Vehicle vehicle = new Vehicle(cf.GetVehicle());

                    // Check if the player is the driver of the vehicle, if so, continue.
                    if (vehicle.GetPedOnSeat(VehicleSeat.Driver) == new Ped(PlayerPedId()))
                    {
                        // Repair vehicle.
                        if (item == fixVehicle)
                        {
                            vehicle.Repair();
                        }
                        // Clean vehicle.
                        else if (item == cleanVehicle)
                        {
                            vehicle.Wash();
                        }
                        //// Delete vehicle.
                        //else if (item == deleteBtn)
                        //{
                        //    vehicle.Delete();
                        //    vehicle = null;
                        //}
                        // Flip vehicle.
                        else if (item == flipVehicle)
                        {
                            SetVehicleOnGroundProperly(vehicle.Handle);
                        }
                        // Toggle alarm.
                        else if (item == vehicleAlarm)
                        {
                            if (vehicle.IsAlarmSounding)
                            {
                                // Set the duration to 0;
                                vehicle.AlarmTimeLeft = 0;
                                vehicle.IsAlarmSet = false;
                            }
                            else
                            {
                                // Randomize duration of the alarm and start the alarm.
                                vehicle.IsAlarmSet = true;
                                vehicle.AlarmTimeLeft = new Random().Next(8000, 45000);
                                vehicle.StartAlarm();
                            }
                        }
                        else if (item == toggleEngine)
                        {
                            vehicle.IsEngineRunning = !vehicle.IsEngineRunning;
                        }
                        else if (item == setLicensePlateText)
                        {
                            cf.SetLicensePlateTextAsync();
                        }
                    }

                    // If the player is not the driver seat and a button other than the option below (cycle seats) was pressed, notify them.
                    else if (item != cycleSeats)
                    {
                        Notify.Error("You must be in the driver seat to access these options!", true, false);
                    }

                    // Only this button can be used when you're not the driver of the car.
                    if (item == cycleSeats)
                    {
                        // Cycle vehicle seats
                        cf.CycleThroughSeats();
                    }
                }
                // If the player is not inside a vehicle, notify them.
                else
                {
                    // Don't notify them, as it doesn't matter anyway. Nothing happens, nothing crashes so it doesn't hurt.
                    //Notify.Error("You must be inside a vehicle to access these options!", true, false);
                }
            };
            #endregion

            #region Handle checkbox changes.
            menu.OnCheckboxChange += (sender, item, _checked) =>
            {
                //### removed because we actually want to handle these changes even if the player is not in a vehicle. ###//
                // ~~If the player is actually in a vehicle, continue.~~
                //if (DoesEntityExist(cf.GetVehicle()))
                //{

                // Create a vehicle object.
                Vehicle vehicle = new Vehicle(cf.GetVehicle());

                if (item == vehicleGod) // God Mode Toggled
                {
                    VehicleGodMode = _checked;
                }
                else if (item == vehicleFreeze) // Freeze Vehicle Toggled
                {
                    VehicleFrozen = _checked;
                }
                else if (item == torqueEnabled) // Enable Torque Multiplier Toggled
                {
                    VehicleTorqueMultiplier = _checked;
                }
                else if (item == powerEnabled) // Enable Power Multiplier Toggled
                {
                    VehiclePowerMultiplier = _checked;
                    if (_checked)
                    {
                        SetVehicleEnginePowerMultiplier(cf.GetVehicle(), VehiclePowerMultiplierAmount);
                    }
                    else
                    {
                        SetVehicleEnginePowerMultiplier(cf.GetVehicle(), 1f);
                    }
                }
                else if (item == vehicleEngineAO) // Leave Engine Running (vehicle always on) Toggled
                {
                    VehicleEngineAlwaysOn = _checked;
                }
                else if (item == vehicleNoSiren) // Disable Siren Toggled
                {
                    VehicleNoSiren = _checked;
                    vehicle.IsSirenSilent = _checked;
                }
                else if (item == vehicleNoBikeHelmet) // No Helemet Toggled
                {
                    VehicleNoBikeHelemet = _checked;
                }

                //}
            };
            #endregion

            #region Handle List Changes.
            // Handle list changes.
            menu.OnListChange += (sender, item, index) =>
            {
                // If the torque multiplier changed. Change the torque multiplier to the new value.
                if (item == torqueMultiplier)
                {
                    // Get the selected value and remove the "x" in the string with nothing.
                    var value = torqueMultiplierList[index].ToString().Replace("x", "");
                    // Convert the value to a float and set it as a public variable.
                    VehicleTorqueMultiplierAmount = float.Parse(value);
                }
                // If the power multiplier is changed. Change the power multiplier to the new value.
                else if (item == powerMultiplier)
                {
                    // Get the selected value. Remove the "x" from the string.
                    var value = powerMultiplierList[index].ToString().Replace("x", "");
                    // Conver the string into a float and set it to be the value of the public variable.
                    VehiclePowerMultiplierAmount = float.Parse(value);
                    if (VehiclePowerMultiplier)
                    {
                        SetVehicleEnginePowerMultiplier(cf.GetVehicle(), VehiclePowerMultiplierAmount);
                    }
                }
                else if (item == setLicensePlateType)
                {
                    // Check if the player is actually in a vehicle.
                    var veh = cf.GetVehicle();
                    if (DoesEntityExist(veh))
                    {
                        Vehicle vehicle = new Vehicle(veh);
                        // Set the license plate style.
                        switch (index)
                        {
                            case 0:
                                vehicle.Mods.LicensePlateStyle = LicensePlateStyle.BlueOnWhite1;
                                break;
                            case 1:
                                vehicle.Mods.LicensePlateStyle = LicensePlateStyle.BlueOnWhite2;
                                break;
                            case 2:
                                vehicle.Mods.LicensePlateStyle = LicensePlateStyle.BlueOnWhite3;
                                break;
                            case 3:
                                vehicle.Mods.LicensePlateStyle = LicensePlateStyle.YellowOnBlue;
                                break;
                            case 4:
                                vehicle.Mods.LicensePlateStyle = LicensePlateStyle.YellowOnBlack;
                                break;
                            case 5:
                                vehicle.Mods.LicensePlateStyle = LicensePlateStyle.NorthYankton;
                                break;
                            default:
                                break;
                        }

                    }
                }
            };
            #endregion

            #region Handle List Items Selected
            menu.OnListSelect += (sender, item, index) =>
            {
                if (item == setDirtLevel)
                {
                    if (IsPedInAnyVehicle(PlayerPedId(), false))
                    {
                        Vehicle veh = new Vehicle(cf.GetVehicle());
                        veh.DirtLevel = float.Parse(index.ToString());
                    }
                }
            };
            #endregion

            #region Vehicle Colors Submenu Stuff

            #region Create lists for each color.
            // Metallic Colors
            List<dynamic> Metallic = new List<dynamic>();
            foreach (KeyValuePair<string, int> color in vd.MetallicColors)
            {
                Metallic.Add(color.Key.ToString());
            }

            // Matte colors
            List<dynamic> Matte = new List<dynamic>();
            foreach (KeyValuePair<string, int> color in vd.MatteColors)
            {
                Matte.Add(color.Key.ToString());
            }

            // Metal colors
            List<dynamic> Metals = new List<dynamic>();
            foreach (KeyValuePair<string, int> color in vd.MetalColors)
            {
                Metals.Add(color.Key.ToString());
            }

            // Util Colors
            List<dynamic> Utils = new List<dynamic>();
            foreach (KeyValuePair<string, int> color in vd.UtilColors)
            {
                Utils.Add(color.Key.ToString());
            }

            // Worn colors
            List<dynamic> Worn = new List<dynamic>();
            foreach (KeyValuePair<string, int> color in vd.WornColors)
            {
                Worn.Add(color.Key.ToString());
            }

            // Pearlescent colors
            List<dynamic> Pearlescent = new List<dynamic>();
            foreach (KeyValuePair<string, int> color in vd.MetallicColors)
            {
                Pearlescent.Add(color.Key.ToString());
            }

            // Wheel colors
            List<dynamic> Wheels = new List<dynamic>();
            Wheels.Add("Default Alloy Color");
            foreach (KeyValuePair<string, int> color in vd.MetallicColors)
            {
                Wheels.Add(color.Key.ToString());
            }
            #endregion

            #region Create the headers + menu list items
            // Headers
            UIMenuItem primaryColorsHeader = new UIMenuItem("~g~Primary Colors:")
            {
                Enabled = false
            };
            UIMenuItem secondaryColorsHeader = new UIMenuItem("~g~Secondary Colors:")
            {
                Enabled = false
            };
            UIMenuItem otherColorsHeader = new UIMenuItem("~g~Other Colors:")
            {
                Enabled = false
            };

            // Primary Colors
            UIMenuListItem classicColors = new UIMenuListItem("Classic", Metallic, 0, "Select a Classic primary color.");
            // Metallic == Classic + Pearlescent
            UIMenuListItem metallicColors = new UIMenuListItem("Metallic", Metallic, 0, "Select a Metallic primary color.");
            UIMenuListItem matteColors = new UIMenuListItem("Matte", Matte, 0, "Select a Matte primary color.");
            UIMenuListItem metalsColors = new UIMenuListItem("Metals", Metals, 0, "Select a Metals primary color.");
            UIMenuListItem utilsColors = new UIMenuListItem("Util", Utils, 0, "Select a Util primary color.");
            UIMenuListItem wornColors = new UIMenuListItem("Worn", Worn, 0, "Select a Worn primary color.");

            // Secondary Colors.
            UIMenuListItem classicColors2 = new UIMenuListItem("Classic", Metallic, 0, "Select a Classic secondary color.");
            UIMenuListItem metallicColors2 = new UIMenuListItem("Metallic", Metallic, 0, "Select a Metallic secondary color.");
            UIMenuListItem matteColors2 = new UIMenuListItem("Matte", Matte, 0, "Select a Matte secondary color.");
            UIMenuListItem metalsColors2 = new UIMenuListItem("Metals", Metals, 0, "Select a Metals secondary color.");
            UIMenuListItem utilsColors2 = new UIMenuListItem("Util", Utils, 0, "Select a Util secondary color.");
            UIMenuListItem wornColors2 = new UIMenuListItem("Worn", Worn, 0, "Select a Worn secondary color.");

            // Other Colors
            // Pearlescent == Classic + Classic on top of secondary color.
            UIMenuListItem pearlescentColors = new UIMenuListItem("Pearlescent", Metallic, 0, "Select a pearlescent color.");
            UIMenuListItem wheelColors = new UIMenuListItem("Wheels Color", Wheels, 0, "Select a color for your wheels.");
            #endregion

            #region Add the items to the colors menu.
            // Primary Colors
            vehicleColors.AddItem(primaryColorsHeader); // header
            vehicleColors.AddItem(classicColors);
            vehicleColors.AddItem(metallicColors);
            vehicleColors.AddItem(matteColors);
            vehicleColors.AddItem(metalsColors);
            vehicleColors.AddItem(utilsColors);
            vehicleColors.AddItem(wornColors);

            // Secondary Colors
            vehicleColors.AddItem(secondaryColorsHeader); // header
            vehicleColors.AddItem(classicColors2);
            vehicleColors.AddItem(metallicColors2);
            vehicleColors.AddItem(matteColors2);
            vehicleColors.AddItem(metalsColors2);
            vehicleColors.AddItem(utilsColors2);
            vehicleColors.AddItem(wornColors2);

            // Other Colors
            vehicleColors.AddItem(otherColorsHeader); // header
            vehicleColors.AddItem(pearlescentColors);
            vehicleColors.AddItem(wheelColors);
            #endregion

            #region Handle Vehicle Color Changes
            vehicleColors.OnListChange += (sender, item, index) =>
            {
                // Get the current vehicle.
                var veh = cf.GetVehicle();

                // Check if the vehicle exists and isn't dead and the player is the driver of the vehicle.
                if (DoesEntityExist(veh) && !IsEntityDead(veh) && GetPedInVehicleSeat(veh, -1) == PlayerPedId())
                {
                    // Get the primary and secondary colors from the current vehicle..
                    int primary = 0;
                    int secondary = 0;
                    int pearlescent = 0;
                    int wheels = 0;
                    GetVehicleColours(veh, ref primary, ref secondary);
                    GetVehicleExtraColours(veh, ref pearlescent, ref wheels);

                    // If any color of the primary colors is selected, which isn't the pearlescent or metallic option, then reset the pearlescent color to black;
                    if (item == classicColors || item == matteColors || item == metalsColors || item == utilsColors || item == wornColors)
                    {
                        pearlescent = 0;
                    }
                    // Classic / Metallic (primary)
                    if (item == classicColors || item == metallicColors)
                    {
                        primary = vd.MetallicColors[Metallic[index]];
                        if (item == metallicColors) // If the primary metallic changes, 
                        {
                            pearlescent = vd.MetallicColors[Metallic[index]];
                        }
                    }
                    // Classic / Metallic (secondary)
                    else if (item == classicColors2 || item == metallicColors2)
                    {
                        secondary = vd.MetallicColors[Metallic[index]];
                    }

                    // Matte (primary)
                    else if (item == matteColors)
                    {
                        primary = vd.MatteColors[Matte[index]];
                    }
                    // Matte (secondary)
                    else if (item == matteColors2)
                    {
                        secondary = vd.MatteColors[Matte[index]];
                    }

                    // Metals (primary)
                    else if (item == metalsColors)
                    {
                        primary = vd.MetalColors[Metals[index]];
                    }
                    // Metals (secondary)
                    else if (item == metalsColors2)
                    {
                        secondary = vd.MetalColors[Metals[index]];
                    }

                    // Utils (primary)
                    else if (item == utilsColors)
                    {
                        primary = vd.UtilColors[Utils[index]];
                    }
                    // Utils (secondary)
                    else if (item == utilsColors2)
                    {
                        secondary = vd.UtilColors[Utils[index]];
                    }

                    // Worn (primary)
                    else if (item == wornColors)
                    {
                        primary = vd.WornColors[Worn[index]];
                    }
                    // Worn (secondary)
                    else if (item == wornColors2)
                    {
                        secondary = vd.WornColors[Worn[index]];
                    }

                    // Pearlescent
                    else if (item == pearlescentColors)
                    {
                        pearlescent = vd.MetallicColors[Metallic[index]];
                    }

                    // Wheel colors
                    else if (item == wheelColors)
                    {
                        if (index == 0)
                        {
                            // "Default Alloy Color" is not in the metallic list, so we have to manually account for this one.
                            wheels = 156;
                        }
                        else
                        {
                            wheels = vd.MetallicColors[Metallic[index - 1]];
                        }
                    }
                    // Set the mod kit so we can modify things.
                    SetVehicleModKit(veh, 0);

                    // Set all the colors.
                    SetVehicleColours(cf.GetVehicle(), primary, secondary);
                    SetVehicleExtraColours(veh, pearlescent, wheels);
                }

            };
            #endregion

            #endregion

            #region Vehicle Doors Submenu Stuff
            UIMenuItem openAll = new UIMenuItem("Open All Doors", "Open all vehicle doors.");
            UIMenuItem closeAll = new UIMenuItem("Close All Doors", "Close all vehicle doors.");
            UIMenuItem LF = new UIMenuItem("Left Front Door", "Open/close the left front door.");
            UIMenuItem RF = new UIMenuItem("Right Front Door", "Open/close the right front door.");
            UIMenuItem LR = new UIMenuItem("Left Rear Door", "Open/close the left rear door.");
            UIMenuItem RR = new UIMenuItem("Right Rear Door", "Open/close the right rear door.");
            UIMenuItem HD = new UIMenuItem("Hood", "Open/close the hood.");
            UIMenuItem TR = new UIMenuItem("Trunk", "Open/close the trunk.");

            vehicleDoorsMenu.AddItem(LF);
            vehicleDoorsMenu.AddItem(RF);
            vehicleDoorsMenu.AddItem(LR);
            vehicleDoorsMenu.AddItem(RR);
            vehicleDoorsMenu.AddItem(HD);
            vehicleDoorsMenu.AddItem(TR);
            vehicleDoorsMenu.AddItem(openAll);
            vehicleDoorsMenu.AddItem(closeAll);

            // Handle button presses.
            vehicleDoorsMenu.OnItemSelect += (sender, item, index) =>
            {
                // Get the vehicle.
                var veh = cf.GetVehicle();
                // If the player is in a vehicle, it's not dead and the player is the driver, continue.
                if (DoesEntityExist(veh) && !IsEntityDead(veh) && GetPedInVehicleSeat(veh, -1) == PlayerPedId())
                {
                    // If button 0-5 are pressed, then open/close that specific index/door.
                    if (index < 6)
                    {
                        // If the door is open.
                        bool open = GetVehicleDoorAngleRatio(veh, index) > 0.1f ? true : false;

                        if (open)
                        {
                            // Close the door.
                            SetVehicleDoorShut(veh, index, false);
                        }
                        else
                        {
                            // Open the door.
                            SetVehicleDoorOpen(veh, index, false, false);
                        }
                    }
                    // If the index >= 6, and the button is "openAll": open all doors.
                    else if (item == openAll)
                    {
                        // Loop through all doors and open them.
                        for (var door = 0; door < 6; door++)
                        {
                            SetVehicleDoorOpen(veh, door, false, false);
                        }
                    }
                    // If the index >= 6, and the button is "closeAll": close all doors.
                    else if (item == closeAll)
                    {
                        // Close all doors.
                        SetVehicleDoorsShut(veh, false);
                    }
                }
                else
                {
                    Notify.Alert("You need to be inside a vehicle to toggle vehicle doors.");
                }

            };

            #endregion

            #region Vehicle Windows Submenu Stuff
            UIMenuItem fwu = new UIMenuItem("↑ Roll Front Windows Up", "Roll both front windows up.");
            UIMenuItem rwu = new UIMenuItem("↑ Roll Rear Windows Up", "Roll both rear windows up.");
            UIMenuItem fwd = new UIMenuItem("↓ Roll Front Windows Down", "Roll both front windows down.");
            UIMenuItem rwd = new UIMenuItem("↓ Roll Rear Windows Down", "Roll both rear windows down.");
            vehicleWindowsMenu.AddItem(fwu);
            vehicleWindowsMenu.AddItem(rwu);
            vehicleWindowsMenu.AddItem(fwd);
            vehicleWindowsMenu.AddItem(rwd);
            vehicleWindowsMenu.OnItemSelect += (sender, item, index) =>
            {
                var veh = cf.GetVehicle();
                if (DoesEntityExist(veh) && !IsEntityDead(veh))
                {
                    if (item == fwu)
                    {
                        RollUpWindow(veh, 0);
                        RollUpWindow(veh, 1);
                    }
                    else if (item == fwd)
                    {
                        RollDownWindow(veh, 0);
                        RollDownWindow(veh, 1);
                    }
                    else if (item == rwu)
                    {
                        RollUpWindow(veh, 2);
                        RollUpWindow(veh, 3);
                    }
                    else if (item == rwd)
                    {
                        RollDownWindow(veh, 2);
                        RollDownWindow(veh, 3);
                    }
                }
            };
            #endregion

            #region Vehicle Liveries Submenu Stuff
            menu.OnItemSelect += (sender, item, idex) =>
            {
                if (item == liveriesMenuBtn)
                {
                    var veh = cf.GetVehicle();
                    if (DoesEntityExist(veh) && !IsEntityDead(veh) && GetPedInVehicleSeat(veh, -1) == PlayerPedId())
                    {
                        //veh = cf.GetVehicle();
                        vehicleLiveries.Clear();
                        SetVehicleModKit(veh, 0);
                        var liveryCount = GetVehicleLiveryCount(veh);

                        if (liveryCount > 0)
                        {
                            //veh = cf.GetVehicle();
                            var liveryList = new List<dynamic>();
                            for (var i = 0; i < liveryCount; i++)
                            {
                                var livery = GetLiveryName(veh, i);
                                livery = GetLabelText(livery) != "NULL" ? GetLabelText(livery) : $"Livery #{i}";
                                liveryList.Add(livery);
                            }
                            UIMenuListItem liveryListItem = new UIMenuListItem("Set Livery", liveryList, GetVehicleLivery(veh), "Choose a livery for this vehicle.");
                            vehicleLiveries.AddItem(liveryListItem);
                            vehicleLiveries.OnListChange += (sender2, item2, index2) =>
                            {
                                veh = cf.GetVehicle();
                                SetVehicleLivery(veh, index2);
                            };
                            vehicleLiveries.RefreshIndex();
                            vehicleLiveries.UpdateScaleform();
                        }
                        else
                        {
                            Notify.Error("This vehicle does not have any liveries.");
                            UIMenuItem backBtn = new UIMenuItem("No liveries for this vehicle :(", "Click me to go back.");
                            backBtn.SetRightLabel("Go Back");
                            vehicleLiveries.AddItem(backBtn);
                            vehicleLiveries.OnItemSelect += (sender2, item2, index2) =>
                            {
                                if (item2 == backBtn)
                                {
                                    vehicleLiveries.GoBack();
                                }
                            };

                            vehicleLiveries.RefreshIndex();
                            vehicleLiveries.UpdateScaleform();
                        }
                        veh = cf.GetVehicle();
                    }
                }

            };
            #endregion

            #region Vehicle Mod Submenu Stuff
            menu.OnItemSelect += (sender, item, index) =>
            {
                // When the mod submenu is openend, reset all items in there.
                if (item == modMenuBtn)
                {
                    // If there are items, remove all of them.
                    if (vehicleModMenu.MenuItems.Count > 0)
                    {
                        vehicleModMenu.Clear();
                    }

                    // Get the vehicle.
                    var veh = cf.GetVehicle();

                    // Check if the vehicle exists, is still drivable/alive and it's actually a vehicle.
                    if (DoesEntityExist(veh) && IsEntityAVehicle(veh) && !IsEntityDead(veh))
                    {
                        // Set the modkit so we can modify the car.
                        SetVehicleModKit(veh, 0);

                        // Create a Vehicle object for it, this is used to get some of the vehicle mods.
                        Vehicle vehicle = new Vehicle(veh);

                        // Get all mods available on this vehicle.
                        var mods = vehicle.Mods.GetAllMods();

                        // Loop through all the mods.
                        foreach (var mod in mods)
                        {
                            // Get the mod type (suspension, armor, etc) name (convert the PascalCase to the Proper Case string values).
                            var typeName = cf.ToProperString(mod.ModType.ToString());

                            // Create a list to all available upgrades for this modtype.
                            var modlist = new List<dynamic>();

                            // Get the current item index ({current}/{max upgrades})
                            var currentItem = $"[1/{ mod.ModCount + 1}]";

                            // Add the stock value for this mod.
                            var name = $"Stock {typeName} {currentItem}";
                            modlist.Add(name);

                            // Loop through all available upgrades for this specific mod type.
                            for (var x = 0; x < mod.ModCount; x++)
                            {
                                // Create the item index.
                                currentItem = $"[{2 + x}/{ mod.ModCount + 1}]";

                                // Create the name (again, converting to proper case), then add the name.
                                name = mod.GetLocalizedModName(x) != "" ? $"{cf.ToProperString(mod.GetLocalizedModName(x))} {currentItem}" : $"{typeName} #{x.ToString()} {currentItem}";
                                modlist.Add(name);
                            }
                            // Create the UIMenuListItem for this mod type.
                            UIMenuListItem modTypeListItem = new UIMenuListItem(typeName, modlist, GetVehicleMod(veh, mod.Index + 1) + 2, $"Choose a ~y~{typeName}~w~ upgrade, it will be automatically applied to your vehicle.");
                            // Add the list item to the menu.
                            vehicleModMenu.AddItem(modTypeListItem);
                        }

                        // Create the wheel types list & listitem and add it to the menu.
                        List<dynamic> wheelTypes = new List<dynamic>() { "Sports", "Muscle", "Lowrider", "SUV", "Offroad", "Tuner", "Bike Wheels", "High End" };
                        UIMenuListItem vehicleWheelType = new UIMenuListItem("Wheel Type", wheelTypes, GetVehicleWheelType(veh), $"Choose a ~y~wheel type~w~ for your vehicle.");
                        vehicleModMenu.AddItem(vehicleWheelType);

                        // Create the checkboxes for some options.
                        UIMenuCheckboxItem toggleCustomWheels = new UIMenuCheckboxItem("Toggle Custom Wheels", GetVehicleModVariation(veh, 23), "Press this to add or remove ~y~custom~w~ wheels.");
                        UIMenuCheckboxItem xenonHeadlights = new UIMenuCheckboxItem("Xenon Headlights", IsToggleModOn(veh, 22), "Enable or disable ~b~xenon ~w~headlights.");
                        UIMenuCheckboxItem turbo = new UIMenuCheckboxItem("Turbo", IsToggleModOn(veh, 18), "Enable or disable the ~y~turbo~w~ for this vehicle.");
                        UIMenuCheckboxItem bulletProofTires = new UIMenuCheckboxItem("Bullet Proof Tires", !GetVehicleTyresCanBurst(veh), "Enable or disable ~y~bullet proof tires~w~ for this vehicle.");

                        // Add the checkboxes to the menu.
                        vehicleModMenu.AddItem(toggleCustomWheels);
                        vehicleModMenu.AddItem(xenonHeadlights);
                        vehicleModMenu.AddItem(turbo);
                        vehicleModMenu.AddItem(bulletProofTires);

                        // Create a list of tire smoke options.
                        List<dynamic> tireSmokes = new List<dynamic>() { "Red", "Orange", "Yellow", "Gold", "Light Green", "Dark Green", "Light Blue", "Dark Blue", "Purple", "Pink", "Black" };
                        Dictionary<String, int[]> tireSmokeColors = new Dictionary<string, int[]>()
                        {
                            ["Red"] = new int[] { 244, 65, 65 },
                            ["Orange"] = new int[] { 244, 167, 66 },
                            ["Yellow"] = new int[] { 244, 217, 65 },
                            ["Gold"] = new int[] { 181, 120, 0 },
                            ["Light Green"] = new int[] { 158, 255, 84 },
                            ["Dark Green"] = new int[] { 44, 94, 5 },
                            ["Light Blue"] = new int[] { 65, 211, 244 },
                            ["Dark Blue"] = new int[] { 24, 54, 163 },
                            ["Purple"] = new int[] { 108, 24, 192 },
                            ["Pink"] = new int[] { 192, 24, 172 },
                            ["Black"] = new int[] { 1, 1, 1 }
                        };
                        UIMenuListItem tireSmoke = new UIMenuListItem("Tire Smoke Color", tireSmokes, GetVehicleWheelType(veh), $"Choose a ~y~wheel type~w~ for your vehicle.");
                        vehicleModMenu.AddItem(tireSmoke);

                        // Create the checkbox to enable/disable the tiresmoke.
                        UIMenuCheckboxItem tireSmokeEnabled = new UIMenuCheckboxItem("Tire Smoke", IsToggleModOn(veh, 20), "Enable or disable ~y~tire smoke~w~.");
                        vehicleModMenu.AddItem(tireSmokeEnabled);

                        // Handle checkbox changes.
                        vehicleModMenu.OnCheckboxChange += (sender2, item2, _checked) =>
                        {
                            // Xenon Headlights
                            if (item2 == xenonHeadlights)
                            {
                                ToggleVehicleMod(veh, 22, _checked);
                            }
                            // Turbo
                            else if (item2 == turbo)
                            {
                                ToggleVehicleMod(veh, 18, _checked);
                            }
                            // Bullet Proof Tires
                            else if (item2 == bulletProofTires)
                            {
                                SetVehicleTyresCanBurst(veh, _checked);
                            }
                            // Custom Wheels
                            else if (item2 == toggleCustomWheels)
                            {
                                veh = cf.GetVehicle();
                                SetVehicleMod(veh, 23, GetVehicleMod(veh, 23), !GetVehicleModVariation(veh, 23));

                                // If the player is on a motorcycle, also change the back wheels.
                                if (IsThisModelABike((uint)GetEntityModel(veh)))
                                {
                                    SetVehicleMod(veh, 24, GetVehicleMod(veh, 24), !GetVehicleModVariation(veh, 23));
                                }
                            }
                            // Toggle Tire Smoke
                            else if (item2 == tireSmokeEnabled)
                            {
                                // If it should be enabled:
                                if (_checked)
                                {
                                    // Enable it.
                                    ToggleVehicleMod(veh, 20, true);
                                    // Get the selected color values.
                                    var r = tireSmokeColors[tireSmokes[tireSmoke.Index]][0];
                                    var g = tireSmokeColors[tireSmokes[tireSmoke.Index]][1];
                                    var b = tireSmokeColors[tireSmokes[tireSmoke.Index]][2];
                                    // Set the color.
                                    SetVehicleTyreSmokeColor(veh, r, g, b);
                                }
                                // If it should be disabled:
                                else
                                {
                                    // Disable it.
                                    ToggleVehicleMod(veh, 20, false);
                                    // Remove the mod.
                                    RemoveVehicleMod(veh, 20);

                                    // Get the current vehicle colors. Changing vehicle colors makes the tire smoke actually reset, 
                                    // without this it would remain the previous color (even if you disable tire smoke).
                                    var prim = 0;
                                    var secn = 0;
                                    GetVehicleColours(veh, ref prim, ref secn);
                                    // Set the vehicle colors.
                                    SetVehicleColours(veh, prim, secn);
                                }
                            }
                        };

                        // Handle list selections
                        vehicleModMenu.OnListChange += (sender2, item2, index2) =>
                        {
                            // If the affected list is actually a "dynamically" generated list, continue. If it was one of the manual options, don't.
                            if (sender2.CurrentSelection < sender2.MenuItems.Count - 7)
                            {
                                veh = cf.GetVehicle();
                                SetVehicleModKit(veh, 0);
                                var dict = new Dictionary<int, int>();
                                var x = 0;
                                foreach (var mod in mods)
                                {
                                    dict.Add(x, (int)mod.ModType);
                                    x++;
                                }
                                var modType = dict[sender2.CurrentSelection];
                                var selectedUpgrade = item2.Index - 1;
                                var wheels = GetVehicleModVariation(veh, 23);

                                SetVehicleMod(veh, modType, selectedUpgrade, wheels);
                            }
                            // It was one of the manual lists/options selected, either vehicle Wheel Type, tire smoke color or window tint:

                            // Wheel types
                            else if (item2 == vehicleWheelType)
                            {
                                // Set the wheel type.
                                veh = cf.GetVehicle();
                                SetVehicleWheelType(veh, index2);
                            }
                            // Tire smoke
                            else if (item2 == tireSmoke)
                            {
                                // Get the selected color values.
                                var r = tireSmokeColors[tireSmokes[index2]][0];
                                var g = tireSmokeColors[tireSmokes[index2]][1];
                                var b = tireSmokeColors[tireSmokes[index2]][2];
                                // Set the color.
                                SetVehicleTyreSmokeColor(veh, r, g, b);
                            }
                        };
                    }
                    // Refresh Index and update the scaleform to prevent weird broken menus.
                    vehicleModMenu.RefreshIndex();
                    vehicleModMenu.UpdateScaleform();
                }
            };
            #endregion
        }
        #endregion

        /// <summary>
        /// Public get method for the menu. Checks if the menu exists, if not create the menu first.
        /// </summary>
        /// <returns>Returns the Vehicle Options menu.</returns>
        public UIMenu GetMenu()
        {
            // If menu doesn't exist. Create one.
            if (menu == null)
            {
                CreateMenu();
            }
            // Return the menu.
            return menu;
        }
    }
}
