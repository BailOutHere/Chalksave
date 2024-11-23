using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ChalkSave.Mods;

public class InjectPlayer : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // "_kiss")
        var connectInject = new MultiTokenWaiter([
            t => t is ConstantToken {Value: StringVariant { Value: "_kiss" } },
            t => t.Type is TokenType.ParenthesisClose
        ]);
        // body.visible = v
        var funcInject = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "body" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "visible" },
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken {Name: "v" }
            ]);


        foreach (var token in tokens)
        {
            
            if (connectInject.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 1);
                // PlayerData.connect("_chalk_list", self, "_chalk_list")
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("connect");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chalk_list"));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_chalk_list"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
            }
            else if (funcInject.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline);
                yield return new Token(TokenType.Newline);
                //func _chalk_list():
                yield return new Token(TokenType.PrFunction);
                yield return new IdentifierToken("_chalk_list");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 1); 
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
                // var CHALK_PATH = "user://chalksaves/"
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("CHALK_PATH");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("user://chalksaves/"));
                yield return new Token(TokenType.Newline, 1);
                // if chalkDirectory.open(CHALK_PATH) == OK:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("open");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("CHALK_PATH");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("OK");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // chalkDirectory.list_dir_begin()
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("list_dir_begin");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                // var file_name = chalkDirectory.get_next()
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("file_name");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_next");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                //while file_name != "":
                yield return new Token(TokenType.CfWhile);
                yield return new IdentifierToken("file_name");
                yield return new Token(TokenType.OpNotEqual);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                // if not chalkDirectory.current_is_dir():
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("current_is_dir");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // Network._update_chat("File: " + file_name)
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("File: "));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("file_name");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                // else:
                /*
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);
                // Network._update_chat("File: " + file_name)
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("File: "));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("file_name");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);
                */
                // file_name = chalkDirectory.get_next()
                yield return new IdentifierToken("file_name");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("chalkDirectory");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("get_next");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // Network._update_chat("An error occurred when trying to access the chalk saves area.")
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("An error occurred when trying to access the chalk saves area."));
                yield return new Token(TokenType.ParenthesisClose); 
                
                yield return new Token(TokenType.Newline);
            }
            else yield return token;
        }
    }
}

