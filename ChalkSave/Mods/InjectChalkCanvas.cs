using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;
using System;
using System.Net;

namespace ChalkSave.Mods;

public class InjectChalkCanvas : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/ChalkCanvas/chalk_canvas.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // "_chalk_send_total")
        var playerDataConnectWaiter = new MultiTokenWaiter([
            t => t is ConstantToken {Value: StringVariant { Value: "_chalk_send_total" } },
            t => t.Type is TokenType.ParenthesisClose,
        ]);
        // if id != canvas_id: return 
        var chalkRecWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken { Name: "id" },
            t => t.Type is TokenType.OpNotEqual,
            t => t is IdentifierToken { Name: "canvas_id" },
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.CfReturn,
            t => t.Type is TokenType.Newline,
        ]);
        // if send_load.size() > 0:
        /*
        DEFUNCT var chalkSendWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken { Name: "send_load" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "size" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.OpGreater,
            t => t is ConstantToken {Value: IntVariant { Value: 0 } },
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline,
        ]); */
        // func _chalk_send_total(id):
        var chalkSendTotalWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken { Name: "_chalk_send_total" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken { Name: "id" },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline,
        ]);

        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);
        // final, color])

        foreach (var token in tokens)
        {
            if (chalkRecWaiter.Check(token))
            {
                yield return token;
                //         Network._update_chat("Chalk recieve triggered.")
                /*
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Chalk recieve triggered."));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                //         Network._update_chat(JSON.print(data))
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("JSON");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("print");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("data");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */
            }
            else if (playerDataConnectWaiter.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 1);
                // PlayerData.connect("_chalk_save", self, "_chalk_save")
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("connect");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chalk_save"));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_chalk_save"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                //PlayerData.connect("_chalk_load", self, "_chalk_load")
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("connect");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chalk_load"));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_chalk_load"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline);
                yield return new Token(TokenType.Newline);
                //func _chalk_save(canvasId, filename):
                yield return new Token(TokenType.PrFunction);
                yield return new IdentifierToken("_chalk_save");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 1);

                // if int(canvasId) != canvas_id: return 
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.BuiltInFunc, 72);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpNotEqual);
                yield return new IdentifierToken("canvas_id");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1);
                // var canvas_arr = []
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // var network_canvas_arr = []
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("network_canvas_arr");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // for cell in get_node("Viewport/TileMap").get_used_cells():
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Viewport/TileMap"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_used_cells");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                //DEFUNCT var color = int(get_node("Viewport/TileMap").get_cell(cell.x, cell.y))
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BuiltInFunc, 72);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Viewport/TileMap"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_cell");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("x");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                // DEFUNCT var color: int = 1
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("int");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));
                */
                //var color = get_node("Viewport/TileMap").get_cell(cell.x, cell.y)

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("int");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Viewport/TileMap"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_cell");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("x");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);

                // DEFUNCT Network._update_chat("x: " + int(cell.x) + " y: " + int(cell.y))
                /*
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("x: "));
                yield return new Token(TokenType.OpAdd);
                yield return new Token(TokenType.BuiltInFunc, 72);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("x");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" y: "));
                yield return new Token(TokenType.OpAdd);
                yield return new Token(TokenType.BuiltInFunc, 72);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                // cell is a vector2....
                // DEFUNCT network_canvas_arr.append([cell, color])
                /*
                yield return new IdentifierToken("network_canvas_arr");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                // DEFUNCT canvas_arr.append([Vector2(cell.x, cell.y), color])
                /*
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("Vector2");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("x");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1); 
                */
                // canvas_arr.append([cell.x, cell.y, color])

                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("x");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // var savename = filename + "_" + canvasId + ".json" 
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("savename");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("_"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(".json"));
                yield return new Token(TokenType.Newline, 1);

                // var chalk_save = {

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.CurlyBracketOpen);
                yield return new Token(TokenType.Newline, 2);
                // "canvasID": int(canvasId),
                yield return new ConstantToken(new StringVariant("canvasID"));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.BuiltInFunc, 72);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 2);
                // "canvasArr": canvas_arr,
                yield return new ConstantToken(new StringVariant("canvasArr"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT "networkCanvasArr": network_canvas_arr
                /*
                yield return new ConstantToken(new StringVariant("networkCanvasArr"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("network_canvas_arr");
                yield return new Token(TokenType.Newline, 1);
                */
                // }
                yield return new Token(TokenType.CurlyBracketClose);
                yield return new Token(TokenType.Newline, 1);

                /*
                // {"canvasID":0,"canvasArr":[["(28,73)",1]]}
                // DEFUNCT var firstItem = true
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("firstItem");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                *//*
                // DEFUNCT var chalk_save = ""
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT var chalk_save = '{"canvasID":' + str(id) + ',"canvasArr":['

                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("{\"canvasID\":"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("String");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(",\"canvasArr\":["));
                yield return new Token(TokenType.Newline, 1);
                
                
                // DEFUNCT for arrEntry in canvas_arr:
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("arrEntry");
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT if not firstItem: chalk_save = chalk_save + ","
                /*
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("firstItem");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(","));
                yield return new Token(TokenType.Newline, 2);
                */ /*
                // DEFUNCT chalk_save = chalk_save + '["(' + str(arrEntry[0]) + ',' + str(arrEntry[1]) + ')",' + str(arrEntry[2]) + ']
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("[\"("));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("String");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("arrEntry");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(","));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("String");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("arrEntry");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(")\","));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("String");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("arrEntry");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("]"));
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT firstItem = false
                /*
                yield return new IdentifierToken("firstItem");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline, 1);
                */ /*
                // DEFUNCT chalk_save = chalk_save + ']}'
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("]}"));
                yield return new Token(TokenType.Newline, 1);
                */
                // var chalkDirectory = Directory.new()
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("Directory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("new");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT var chalkDirectory = DirAccess.open("user://")
                //yield return new Token(TokenType.PrVar);
                //yield return new IdentifierToken("chalkDirectory");
                //yield return new Token(TokenType.OpAssign);
                //yield return new IdentifierToken("DirAccess");
                //yield return new Token(TokenType.Period);
                //yield return new IdentifierToken("open");
                //yield return new Token(TokenType.ParenthesisOpen);
                //yield return new ConstantToken(new StringVariant("user://"));
                //yield return new Token(TokenType.ParenthesisClose);
                //yield return new Token(TokenType.Newline, 1);  /*

                // chalkDirectory.open("user://")
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("open");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("user://"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // chalkDirectory.make_dir("chalksaves") 
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("make_dir");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("chalksaves"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // var finalFilename = "user://chalksaves/" + savename
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("finalFilename");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("user://chalksaves/"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("savename");
                yield return new Token(TokenType.Newline, 1);
                // var chalkSave = File.new()
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("File");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("new");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // chalkSave.open(finalFilename, File.WRITE)
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("open");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("finalFilename");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("File");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("WRITE");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT chalkSave.store_var(chalk_save)
                /*
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("store_var");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // chalkSave.store_line(JSON.print(chalk_save))
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("store_line");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("JSON");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("print");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // DEFUNCT chalkSave.store_line(chalk_save)
                /*
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("store_string");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // chalkSave.close()
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("close");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // Network._update_chat("Saved file: " + savename)
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Saved file: "));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("savename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline);

                yield return new Token(TokenType.Newline);
                //func _chalk_load(filename):
                yield return new Token(TokenType.PrFunction);
                yield return new IdentifierToken("_chalk_load");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 1);
                // var CHALK_SAVE_PATH = "user://chalksaves/" + filename
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("CHALK_SAVE_PATH");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("user://chalksaves/"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.Newline, 1);
                //	var chalkSave = File.new()
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("File");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("new");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                //	var chalkSave_exists = chalkSave.file_exists(CHALK_SAVE_PATH)
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkSave_exists");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("file_exists");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("CHALK_SAVE_PATH");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                //     if chalkSave_exists == false:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("chalkSave_exists");
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                //         Network._update_chat("Chalk save does not exist.")
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Chalk save does not exist."));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                // return 
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1);
                // var chalk_loaded_save = {}

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.CurlyBracketOpen);
                yield return new Token(TokenType.CurlyBracketClose);
                yield return new Token(TokenType.Newline, 1);

                // DEFUNCT var chalk_loaded_save = {
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.CurlyBracketOpen);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT "canvasID": -1,
                yield return new ConstantToken(new StringVariant("canvasID"));
                yield return new Token(TokenType.Colon);
                yield return new ConstantToken(new IntVariant(-1));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT "canvasArr": []
                yield return new ConstantToken(new StringVariant("canvasArr"));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT }
                yield return new Token(TokenType.CurlyBracketClose);
                yield return new Token(TokenType.Newline, 1);
                */
                //     chalkSave.open(CHALK_SAVE_PATH, File.READ)
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("open");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("CHALK_SAVE_PATH");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("File");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("READ");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT    chalk_loaded_save = chalkSave.get_var()
                /*
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_var");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // var json_chalk_loaded_save = JSON.parse(chalkSave.get_line())
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("json_chalk_loaded_save");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("JSON");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("parse");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_line");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                //     chalkSave.close()
                yield return new IdentifierToken("chalkSave");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("close");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // if json_chalk_loaded_save.error != 0: /n2
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("json_chalk_loaded_save");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("error");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // 		 Network._update_chat("Chalk save broken.")
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Chalk save broken."));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                // return
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1);
                // chalk_loaded_save = json_chalk_loaded_save.result
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("json_chalk_loaded_save");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("result");
                yield return new Token(TokenType.Newline, 1);

                /*
                // 	DEFUNCT if chalk_loaded_save == null:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.OpEqual);
                yield return new Token(TokenType.PrVoid);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // 	DEFUNCT	 Network._update_chat("Chalk save broken.")
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Chalk save broken."));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                //DEFUNCT return
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1); */

                // var SendSaveCells = []
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT var networkSendSaveCells = []
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("networkSendSaveCells");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // SendSaveCells = chalk_loaded_save["canvasArr"]
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("canvasArr"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT networkSendSaveCells = chalk_loaded_save["networkCanvasArr"]
                /*
                yield return new IdentifierToken("networkSendSaveCells");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("networkCanvasArr"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // var canvasID = chalk_loaded_save["canvasID"]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("canvasID");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("canvasID"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // if canvasID != canvas_id: return 
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("canvasID");
                yield return new Token(TokenType.OpNotEqual);
                yield return new IdentifierToken("canvas_id");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1);
                //  Network._update_chat("Loading file: " + filename)

                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Loading file: "));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT send_load += chalk_loaded_save["networkCanvasArr"]
                /*
                yield return new IdentifierToken("send_load");
                yield return new Token(TokenType.OpAssignAdd);
                yield return new IdentifierToken("chalk_loaded_save");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("networkCanvasArr"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT _chalk_send()
                yield return new IdentifierToken("_chalk_send");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // DEFUNCT var canvas_arr = []
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // DEFUNCT for cell in SendSaveCells:
                /*
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT canvas_arr.append([Vector2(float(cell[0]), float(cell[1])), cell[2]])
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("Vector2");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("float");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("float");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */
                // DEFUNCT if SendSaveCells.size() > 0:
                /*
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT var chalk_save = {

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalk_save");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.CurlyBracketOpen);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT "canvasID": int(canvasId),
                yield return new ConstantToken(new StringVariant("canvasID"));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.BuiltInFunc, 72);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT "canvasArr": canvas_arr,
                yield return new ConstantToken(new StringVariant("canvasArr"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("canvas_arr");
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT "networkCanvasArr": network_canvas_arr

                yield return new ConstantToken(new StringVariant("networkCanvasArr"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("network_canvas_arr");
                yield return new Token(TokenType.Newline, 1);

                // }
                yield return new Token(TokenType.CurlyBracketClose);
                yield return new Token(TokenType.Newline, 1);
                */
                /*
                // DEFUNCT var network_dict = {
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("network_dict");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.CurlyBracketOpen);
                yield return new Token(TokenType.Newline, 2);
                //  DEFUNCT  "type": "chalk_packet",
                yield return new ConstantToken(new StringVariant("type"));
                yield return new Token(TokenType.Colon);
                yield return new ConstantToken(new StringVariant("chalk_packet"));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 2);
                //  DEFUNCT  "data": networkSendSaveCells.duplicate(),
                yield return new ConstantToken(new StringVariant("data"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("networkSendSaveCells");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("duplicate");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Newline, 2);
                // DEFUNCT   "canvas_id": canvas_id
                yield return new ConstantToken(new StringVariant("canvas_id"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("canvas_id");
                yield return new Token(TokenType.Newline, 2);
                //}
                yield return new Token(TokenType.CurlyBracketClose);
                yield return new Token(TokenType.Newline, 1);
                // DEFUNCT send_load += networkSendSaveCells
                /*
                yield return new IdentifierToken("canvas_id");
                yield return new Token(TokenType.OpAssignAdd);
                yield return new IdentifierToken("networkSendSaveCells");
                yield return new Token(TokenType.Newline, 1);
                
                // DEFUNCT _chalk_send()
                yield return new IdentifierToken("_chalk_send");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                */

                // DEFUNCT Network._send_P2P_Packet({"type": "chalk_packet", "data": networkSendSaveCells.duplicate(), networkSendSaveCells.duplicate() canvas_id}, "all", 2, Network.CHANNELS.CHALK)
                // DEFUNCT Network._send_P2P_Packet(network_dict, "all", 2, Network.CHANNELS.CHALK)
                /*
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_P2P_Packet");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("network_dict");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("all"));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("CHANNELS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("CHALK");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                //    DEFUNCT     Network._update_chat(JSON.print(SendSaveCells))
                /*
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("JSON");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("print");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                // for celldata in SendSaveCells:
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("celldata");
                yield return new Token(TokenType.OpIn);
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                //   DEFUNCT  var cell = _clamp_cell(celldata[0], 0)
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("_clamp_cell");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("celldata");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                */
                //  DEFUNCT   var cell = celldata[0]
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("celldata");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);
                */
                //     var cell_x = celldata[0]

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("cell_x");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("celldata");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 2);
                //     var cell_y = celldata[1]

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("cell_y");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("celldata");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 2);
                //     var Cellcolor = celldata[2]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("Cellcolor");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("celldata");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 2);
                //  DEFUNCT   if not (Cellcolor is int): Cellcolor = 0
                /*
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("Cellcolor");
                yield return new Token(TokenType.PrIs);
                yield return new IdentifierToken("int");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("color");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Newline, 3);
                */
                //    get_node("Viewport/TileMap").set_cell(cell.x, cell.y, Cellcolor)
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Viewport/TileMap"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("set_cell");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell_x");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("cell_y");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("Cellcolor");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                //  DEFUNCT  get_node("Viewport/TileMap").set_cell(54, 82, 4) -- WORKS!!!
                /*
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Viewport/TileMap"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("set_cell");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new IntVariant(54));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(82));

                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(4));

                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                // DEFUNCT var x_str = str(cell.x)
                /*
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("x_str");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("str");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("cell");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("x");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // DEFUNCT Network._update_chat("x: " + String(cell.x) + " y: " + String(cell.x))
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("x: "));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("x_str");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                // DEFUNCT _chalk_recieve(SendSaveCells, canvasID)
                /*
                yield return new IdentifierToken("_chalk_recieve");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("SendSaveCells");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("duplicate");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("canvasID");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                */
                //  Network._update_chat(filename + " loaded!")
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" loaded!"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // _chalk_send_total("all")
                yield return new IdentifierToken("_chalk_send_total");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("all"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);



            }/*
            DEFUNCT else if (chalkSendWaiter.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 2);
                // Network._update_chat(JSON.print(send_load))
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("JSON");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("print");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("send_load");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
            }*/
            else if (chalkSendTotalWaiter.Check(token))
            {
                yield return token;
                newlineConsumer.SetReady();
            }
            else if (newlineConsumer.Check(token))
            {
                continue;
            }
            else yield return token;
        }
    }
}

