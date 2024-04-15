﻿using FluentAssertions;
using Moq;
using Moq.Protected;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Implementations;
using System.Net;
using Xunit;

namespace SteamWebWrapper.UnitTests.WebWrapper;

public class GetMarketHistoryTests
{
	private SteamHttpClient SteamHttpClient { get; set; }
	private MarketWrapper MarketWrapper { get; set; }
	private Mock<HttpClientHandler> MockedHandler { get; set; }

	public GetMarketHistoryTests()
	{
		MockedHandler = new Mock<HttpClientHandler>();
		SteamHttpClient = new SteamHttpClient(MockedHandler.Object);
		MarketWrapper = new MarketWrapper(SteamHttpClient);
	}

	[Fact]
	public async Task GetMarketHistorySuccess()
	{
		const string rawResponse = "{\n    \"success\": true,\n    \"pagesize\": 5,\n    \"total_count\": 11053,\n    \"start\": 5,\n    \"assets\": {\n        \"730\": {\n            \"2\": {\n                \"31513974330\": {\n                    \"currency\": 0,\n                    \"appid\": 730,\n                    \"contextid\": \"2\",\n                    \"id\": \"31513974330\",\n                    \"classid\": \"4726069035\",\n                    \"instanceid\": \"188530139\",\n                    \"amount\": \"0\",\n                    \"status\": 4,\n                    \"original_amount\": \"1\",\n                    \"unowned_id\": \"31513974330\",\n                    \"unowned_contextid\": \"2\",\n                    \"background_color\": \"\",\n                    \"icon_url\": \"-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXH5ApeO4YmlhxYQknCRvCo04DEVlxkKgpou7umeldf0Ob3fDxBvYyJhImTnvLLPr7Vn35cpscj37CS996g3FHj-EBpYWr6coPGcQFqZgqE8lPqle3pjJK5uJqbz3F9-n51qYN3NaA\",\n                    \"descriptions\": [\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Exterior: Field-Tested\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Essentially a box that bullets come out of, the MAC-10 SMG boasts a high rate of fire, with poor spread accuracy and high recoil as trade-offs. A custom paint job of pixies, trapped inside the weapon and eager to escape, has been applied.\\n\\n<i>They have yet to find someone they cannot fool</i>\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"The Dreams & Nightmares Collection\",\n                            \"color\": \"9da1a9\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        }\n                    ],\n                    \"tradable\": 1,\n                    \"actions\": [\n                        {\n                            \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M4726033880759075406A%assetid%D9269007165165791883\",\n                            \"name\": \"Inspect in Game...\"\n                        }\n                    ],\n                    \"name\": \"MAC-10 | Ensnared\",\n                    \"name_color\": \"D2D2D2\",\n                    \"type\": \"Mil-Spec Grade SMG\",\n                    \"market_name\": \"MAC-10 | Ensnared (Field-Tested)\",\n                    \"market_hash_name\": \"MAC-10 | Ensnared (Field-Tested)\",\n                    \"market_actions\": [\n                        {\n                            \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M4726033880759075406A%assetid%D9269007165165791883\",\n                            \"name\": \"Inspect in Game...\"\n                        }\n                    ],\n                    \"commodity\": 0,\n                    \"market_tradable_restriction\": 7,\n                    \"marketable\": 1,\n                    \"app_icon\": \"https://cdn.akamai.steamstatic.com/steamcommunity/public/images/apps/730/8dbc71957312bbd3baea65848b545be9eae2a355.jpg\",\n                    \"owner\": 0\n                },\n                \"29526347016\": {\n                    \"currency\": 0,\n                    \"appid\": 730,\n                    \"contextid\": \"2\",\n                    \"id\": \"29526347016\",\n                    \"classid\": \"2948874694\",\n                    \"instanceid\": \"0\",\n                    \"amount\": \"0\",\n                    \"status\": 4,\n                    \"original_amount\": \"1\",\n                    \"unowned_id\": \"29526347016\",\n                    \"unowned_contextid\": \"2\",\n                    \"background_color\": \"\",\n                    \"icon_url\": \"-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXU5A1PIYQNqhpOSV-fRPasw8rsUFJ5KBFZv668FFUwnfbOdDgavYXukYTZkqf2ZbrTwmkE6scgj7CY94ml3FXl-ENkMW3wctOLMlhpVHKV9YA\",\n                    \"descriptions\": [\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Container Series #244\",\n                            \"color\": \"99ccff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Contains one of the following:\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"AUG | Amber Slipstream\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Dual Berettas | Shred\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Glock-18 | Warhawk\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"MP9 | Capillary\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"P90 | Traction\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"R8 Revolver | Survivalist\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Tec-9 | Snek-9\",\n                            \"color\": \"4b69ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"CZ75-Auto | Eco\",\n                            \"color\": \"8847ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"G3SG1 | High Seas\",\n                            \"color\": \"8847ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Nova | Toy Soldier\",\n                            \"color\": \"8847ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"AWP | PAW\",\n                            \"color\": \"8847ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"MP7 | Powercore\",\n                            \"color\": \"8847ff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"M4A1-S | Nightmare\",\n                            \"color\": \"d32ce6\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Sawed-Off | Devourer\",\n                            \"color\": \"d32ce6\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"FAMAS | Eye of Athena\",\n                            \"color\": \"d32ce6\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"AK-47 | Neon Rider\",\n                            \"color\": \"eb4b4b\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Desert Eagle | Code Red\",\n                            \"color\": \"eb4b4b\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"or an Exceedingly Rare Special Item!\",\n                            \"color\": \"ffd700\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"\",\n                            \"color\": \"00a000\"\n                        }\n                    ],\n                    \"tradable\": 1,\n                    \"name\": \"Horizon Case\",\n                    \"name_color\": \"D2D2D2\",\n                    \"type\": \"Base Grade Container\",\n                    \"market_name\": \"Horizon Case\",\n                    \"market_hash_name\": \"Horizon Case\",\n                    \"commodity\": 1,\n                    \"market_tradable_restriction\": 7,\n                    \"marketable\": 1,\n                    \"market_buy_country_restriction\": \"FR\",\n                    \"app_icon\": \"https://cdn.akamai.steamstatic.com/steamcommunity/public/images/apps/730/8dbc71957312bbd3baea65848b545be9eae2a355.jpg\",\n                    \"owner\": 0\n                },\n                \"31436688750\": {\n                    \"currency\": 0,\n                    \"appid\": 730,\n                    \"contextid\": \"2\",\n                    \"id\": \"31436688750\",\n                    \"classid\": \"3035569529\",\n                    \"instanceid\": \"302028390\",\n                    \"amount\": \"0\",\n                    \"status\": 4,\n                    \"original_amount\": \"1\",\n                    \"unowned_id\": \"31436688750\",\n                    \"unowned_contextid\": \"2\",\n                    \"background_color\": \"\",\n                    \"icon_url\": \"-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXH5ApeO4YmlhxYQknCRvCo04DEVlxkKgpoo7e1f1Jf2-r3czFX6cyknY6fqPX4Jr7Dk29u5cB1g_zMu9ygiQDk_RU_YziiIdLAdlJvMljXrwXsxbvugJDov5nAy3I17igqtn7D30vgJD2EjJ0\",\n                    \"descriptions\": [\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Exterior: Field-Tested\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"The misunderstood middle child of the SMG family, the UMP45's small magazine is the only drawback to an otherwise versatile close-quarters automatic. A blue hydrographic pattern resembling a nuclear power plant has been applied.\\n\\n<i>Cut the lights</i>\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"The 2018 Nuke Collection\",\n                            \"color\": \"9da1a9\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        }\n                    ],\n                    \"tradable\": 1,\n                    \"actions\": [\n                        {\n                            \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M4726033880759076066A%assetid%D1048990094554275528\",\n                            \"name\": \"Inspect in Game...\"\n                        }\n                    ],\n                    \"name\": \"UMP-45 | Facility Dark\",\n                    \"name_color\": \"D2D2D2\",\n                    \"type\": \"Consumer Grade SMG\",\n                    \"market_name\": \"UMP-45 | Facility Dark (Field-Tested)\",\n                    \"market_hash_name\": \"UMP-45 | Facility Dark (Field-Tested)\",\n                    \"market_actions\": [\n                        {\n                            \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M4726033880759076066A%assetid%D1048990094554275528\",\n                            \"name\": \"Inspect in Game...\"\n                        }\n                    ],\n                    \"commodity\": 0,\n                    \"market_tradable_restriction\": 7,\n                    \"marketable\": 1,\n                    \"app_icon\": \"https://cdn.akamai.steamstatic.com/steamcommunity/public/images/apps/730/8dbc71957312bbd3baea65848b545be9eae2a355.jpg\",\n                    \"owner\": 0\n                },\n                \"29578509650\": {\n                    \"currency\": 0,\n                    \"appid\": 730,\n                    \"contextid\": \"2\",\n                    \"id\": \"29578509650\",\n                    \"classid\": \"3232289007\",\n                    \"instanceid\": \"188530398\",\n                    \"amount\": \"0\",\n                    \"status\": 4,\n                    \"original_amount\": \"1\",\n                    \"unowned_id\": \"29578509650\",\n                    \"unowned_contextid\": \"2\",\n                    \"background_color\": \"\",\n                    \"icon_url\": \"-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXH5ApeO4YmlhxYQknCRvCo04DEVlxkKgpovrG1eVcwg8zLZAJSvozmxL-ehfX1PYTZl3FQ-sFOh-zF_Jn4xlftr0toNTv1coSWIFdvM1rW_AC5lern1JS6vJnLySMwv3Vw5ynbnxGpwUYb5mDlOt8\",\n                    \"descriptions\": [\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Exterior: Field-Tested\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"This item features StatTrak\u2122 technology, which tracks certain statistics when equipped by its owner.\",\n                            \"color\": \"99ccff\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"StatTrak\u2122 Confirmed Kills: 2\",\n                            \"color\": \"CF6A32\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"*Stats for this item will reset when used in Steam Trading or Community Market\",\n                            \"color\": \"ff4040\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"Accurate and controllable, the German-made P2000 is a serviceable first-round pistol that works best against unarmored opponents. It has been custom painted with a grayscale camo grip and a red slide.\\n\\n<i>Hard hats required beyond this point</i>\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"The Clutch Collection\",\n                            \"color\": \"9da1a9\"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \" \"\n                        },\n                        {\n                            \"type\": \"html\",\n                            \"value\": \"<br><div id=\\\"sticker_info\\\" name=\\\"sticker_info\\\" title=\\\"Sticker\\\" style=\\\"border: 2px solid rgb(102, 102, 102); border-radius: 6px; width=100; margin:4px; padding:8px;\\\"><center><img width=64 height=48 src=\\\"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/stickers/katowice2019/sig_adrenkz_foil.8661e9506aae09c1778297d043f6e2295d8b0490.png\\\"><img width=64 height=48 src=\\\"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/stickers/katowice2019/faze.9883b325d14d4ef4795b6b54f511e66d8133d940.png\\\"><img width=64 height=48 src=\\\"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/stickers/katowice2019/faze.9883b325d14d4ef4795b6b54f511e66d8133d940.png\\\"><br>Sticker: AdreN (Foil) | Katowice 2019, FaZe Clan | Katowice 2019, FaZe Clan | Katowice 2019</center></div>\"\n                        }\n                    ],\n                    \"tradable\": 1,\n                    \"actions\": [\n                        {\n                            \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M4726033880759075886A%assetid%D11829916951440523424\",\n                            \"name\": \"Inspect in Game...\"\n                        }\n                    ],\n                    \"name\": \"StatTrak\u2122 P2000 | Urban Hazard\",\n                    \"name_color\": \"CF6A32\",\n                    \"type\": \"StatTrak\u2122 Mil-Spec Grade Pistol\",\n                    \"market_name\": \"StatTrak\u2122 P2000 | Urban Hazard (Field-Tested)\",\n                    \"market_hash_name\": \"StatTrak\u2122 P2000 | Urban Hazard (Field-Tested)\",\n                    \"market_actions\": [\n                        {\n                            \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M4726033880759075886A%assetid%D11829916951440523424\",\n                            \"name\": \"Inspect in Game...\"\n                        }\n                    ],\n                    \"commodity\": 0,\n                    \"market_tradable_restriction\": 7,\n                    \"marketable\": 1,\n                    \"app_icon\": \"https://cdn.akamai.steamstatic.com/steamcommunity/public/images/apps/730/8dbc71957312bbd3baea65848b545be9eae2a355.jpg\",\n                    \"owner\": 0\n                }\n            }\n        }\n    },\n    \"events\": [\n        {\n            \"listingid\": \"4726033880759075406\",\n            \"purchaseid\": \"4726033880759075407\",\n            \"event_type\": 3,\n            \"time_event\": 1707686079,\n            \"time_event_fraction\": 307000000,\n            \"steamid_actor\": \"76561199517946921\",\n            \"date_event\": \"11 Feb\"\n        },\n        {\n            \"listingid\": \"4726033880759080596\",\n            \"purchaseid\": \"4726033880759080597\",\n            \"event_type\": 3,\n            \"time_event\": 1707686045,\n            \"time_event_fraction\": 160000000,\n            \"steamid_actor\": \"76561198124423567\",\n            \"date_event\": \"11 Feb\"\n        },\n        {\n            \"listingid\": \"4726033880759076066\",\n            \"purchaseid\": \"4726033880759076067\",\n            \"event_type\": 3,\n            \"time_event\": 1707686003,\n            \"time_event_fraction\": 210000000,\n            \"steamid_actor\": \"76561198043803220\",\n            \"date_event\": \"11 Feb\"\n        },\n        {\n            \"listingid\": \"4726033880759075886\",\n            \"purchaseid\": \"4726033880759075887\",\n            \"event_type\": 3,\n            \"time_event\": 1707685985,\n            \"time_event_fraction\": 730000000,\n            \"steamid_actor\": \"76561198201559583\",\n            \"date_event\": \"11 Feb\"\n        },\n        {\n            \"listingid\": \"4726033880759080596\",\n            \"event_type\": 1,\n            \"time_event\": 1707685958,\n            \"time_event_fraction\": 493000000,\n            \"steamid_actor\": \"76561198040697804\",\n            \"date_event\": \"11 Feb\"\n        }\n    ],\n    \"purchases\": {\n        \"4726033880759075406_4726033880759075407\": {\n            \"listingid\": \"4726033880759075406\",\n            \"purchaseid\": \"4726033880759075407\",\n            \"time_sold\": 1707686079,\n            \"steamid_purchaser\": \"76561199517946921\",\n            \"needs_rollback\": 0,\n            \"failed\": 0,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"31513974330\",\n                \"classid\": \"4726069035\",\n                \"instanceid\": \"188530139\",\n                \"amount\": \"1\",\n                \"status\": 4,\n                \"new_id\": \"35793721499\",\n                \"new_contextid\": \"2\"\n            },\n            \"paid_amount\": 4,\n            \"paid_fee\": 2,\n            \"currencyid\": \"2001\",\n            \"steam_fee\": 1,\n            \"publisher_fee\": 1,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"publisher_fee_app\": 730,\n            \"received_amount\": 1789,\n            \"received_currencyid\": \"2037\",\n            \"funds_returned\": 0,\n            \"avatar_actor\": \"https://avatars.akamai.steamstatic.com/a8666f4c04106ab82e47d0cec511e17fc1b89200.jpg\",\n            \"persona_actor\": \"Aura\"\n        },\n        \"4726033880759080596_4726033880759080597\": {\n            \"listingid\": \"4726033880759080596\",\n            \"purchaseid\": \"4726033880759080597\",\n            \"time_sold\": 1707686045,\n            \"steamid_purchaser\": \"76561198124423567\",\n            \"needs_rollback\": 0,\n            \"failed\": 0,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"29526347016\",\n                \"classid\": \"2948874694\",\n                \"instanceid\": \"0\",\n                \"amount\": \"1\",\n                \"status\": 4,\n                \"new_id\": \"35793715124\",\n                \"new_contextid\": \"2\"\n            },\n            \"paid_amount\": 33726,\n            \"paid_fee\": 5058,\n            \"currencyid\": \"2037\",\n            \"steam_fee\": 1686,\n            \"publisher_fee\": 3372,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"publisher_fee_app\": 730,\n            \"received_amount\": 33726,\n            \"received_currencyid\": \"2037\",\n            \"funds_returned\": 0,\n            \"avatar_actor\": \"https://avatars.akamai.steamstatic.com/9d0d3ccddb2abcd20881289516e8744bb673f3b0.jpg\",\n            \"persona_actor\": \"IGORЬ\"\n        },\n        \"4726033880759076066_4726033880759076067\": {\n            \"listingid\": \"4726033880759076066\",\n            \"purchaseid\": \"4726033880759076067\",\n            \"time_sold\": 1707686003,\n            \"steamid_purchaser\": \"76561198043803220\",\n            \"needs_rollback\": 0,\n            \"failed\": 0,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"31436688750\",\n                \"classid\": \"3035569529\",\n                \"instanceid\": \"302028390\",\n                \"amount\": \"1\",\n                \"status\": 4,\n                \"new_id\": \"35793706641\",\n                \"new_contextid\": \"2\"\n            },\n            \"paid_amount\": 43,\n            \"paid_fee\": 6,\n            \"currencyid\": \"2005\",\n            \"steam_fee\": 2,\n            \"publisher_fee\": 4,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"publisher_fee_app\": 730,\n            \"received_amount\": 211,\n            \"received_currencyid\": \"2037\",\n            \"funds_returned\": 0,\n            \"avatar_actor\": \"https://avatars.akamai.steamstatic.com/80cff20d5082ddf1a393595dd853644e7e368abb.jpg\",\n            \"persona_actor\": \"Солевой вездеход\"\n        },\n        \"4726033880759075886_4726033880759075887\": {\n            \"listingid\": \"4726033880759075886\",\n            \"purchaseid\": \"4726033880759075887\",\n            \"time_sold\": 1707685985,\n            \"steamid_purchaser\": \"76561198201559583\",\n            \"needs_rollback\": 0,\n            \"failed\": 0,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"29578509650\",\n                \"classid\": \"3232289007\",\n                \"instanceid\": \"188530398\",\n                \"amount\": \"1\",\n                \"status\": 4,\n                \"new_id\": \"35793702784\",\n                \"new_contextid\": \"2\"\n            },\n            \"paid_amount\": 820,\n            \"paid_fee\": 123,\n            \"currencyid\": \"2005\",\n            \"steam_fee\": 41,\n            \"publisher_fee\": 82,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"publisher_fee_app\": 730,\n            \"received_amount\": 4025,\n            \"received_currencyid\": \"2037\",\n            \"funds_returned\": 0,\n            \"avatar_actor\": \"https://avatars.akamai.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb.jpg\",\n            \"persona_actor\": \"Alex_Smurf\"\n        }\n    },\n    \"listings\": {\n        \"4726033880759075406\": {\n            \"listingid\": \"4726033880759075406\",\n            \"price\": 0,\n            \"fee\": 0,\n            \"publisher_fee_app\": 730,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"currencyid\": 2037,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"31513974330\",\n                \"amount\": \"0\",\n                \"market_actions\": [\n                    {\n                        \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M%listingid%A%assetid%D9269007165165791883\",\n                        \"name\": \"Inspect in Game...\"\n                    }\n                ]\n            },\n            \"original_price\": 1786\n        },\n        \"4726033880759080596\": {\n            \"listingid\": \"4726033880759080596\",\n            \"price\": 0,\n            \"fee\": 0,\n            \"publisher_fee_app\": 730,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"currencyid\": 2037,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"29526347016\",\n                \"amount\": \"0\"\n            },\n            \"original_price\": 33726\n        },\n        \"4726033880759076066\": {\n            \"listingid\": \"4726033880759076066\",\n            \"price\": 0,\n            \"fee\": 0,\n            \"publisher_fee_app\": 730,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"currencyid\": 2037,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"31436688750\",\n                \"amount\": \"0\",\n                \"market_actions\": [\n                    {\n                        \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M%listingid%A%assetid%D1048990094554275528\",\n                        \"name\": \"Inspect in Game...\"\n                    }\n                ]\n            },\n            \"original_price\": 211\n        },\n        \"4726033880759075886\": {\n            \"listingid\": \"4726033880759075886\",\n            \"price\": 0,\n            \"fee\": 0,\n            \"publisher_fee_app\": 730,\n            \"publisher_fee_percent\": \"0.100000001490116119\",\n            \"currencyid\": 2037,\n            \"asset\": {\n                \"currency\": 0,\n                \"appid\": 730,\n                \"contextid\": \"2\",\n                \"id\": \"29578509650\",\n                \"amount\": \"0\",\n                \"market_actions\": [\n                    {\n                        \"link\": \"steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20M%listingid%A%assetid%D11829916951440523424\",\n                        \"name\": \"Inspect in Game...\"\n                    }\n                ]\n            },\n            \"original_price\": 4025\n        }\n    }\n}";
		const string reqUri = $"https://steamcommunity.com/market/myhistory/?query=&count=5&start=5&norender=true";
		var httpRequestMessage = ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.Method == HttpMethod.Get && 
		                                                          req.RequestUri.AbsoluteUri.Equals(reqUri, StringComparison.OrdinalIgnoreCase));
		
		var httpResponseMessage = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(rawResponse)
		});
		
		MockedHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync",httpRequestMessage, ItExpr.IsAny<CancellationToken>())
			.Returns(httpResponseMessage);

		const int count = 5;
		const int offset = 5;
		var cancellationTokenSource = new CancellationTokenSource();
		
		var response = await MarketWrapper.GetMarketHistoryAsync(offset, count, cancellationTokenSource.Token);

		const int totalCount = 11053;
		const int assetCount = 4;
		
		response.Should().NotBeNull();
		response.Success.Should().BeTrue();
		response.Assets.Should().HaveCount(assetCount);
		response.PageSize.Should().Be(count);
		response.Start.Should().Be(offset);
		response.TotalCount.Should().Be(totalCount);
	}
	
	[Fact]
	public async Task GetMarketHistory_ThrowsTooManyRequestsHttpException()
	{
		const string reqUri = $"https://steamcommunity.com/market/myhistory/?query=&count=50&start=5&norender=true";
		var httpRequestMessage = ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.Method == HttpMethod.Get && 
		                                                          req.RequestUri.AbsoluteUri.Equals(reqUri, StringComparison.OrdinalIgnoreCase));
		
		var httpResponseMessage = Task.FromResult(new HttpResponseMessage(HttpStatusCode.TooManyRequests));
		
		MockedHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync",httpRequestMessage, ItExpr.IsAny<CancellationToken>())
			.Returns(httpResponseMessage);

		const int count = 50;
		const int offset = 5;
		var cancellationTokenSource = new CancellationTokenSource();
		
		var action = async () => await MarketWrapper.GetMarketHistoryAsync(offset, count, cancellationTokenSource.Token);
		var exc = await action.Should()
			.ThrowAsync<HttpRequestException>();
		
		exc.Which.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
	}
}