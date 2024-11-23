using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ChalkSave.Mods;

public class InjectPlayerHud : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";
    // NodePath
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var commandsInject = new MultiTokenWaiter([
            t => t.Type is TokenType.CfMatch,
            t => t is IdentifierToken { Name: "line" },
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline

        ]);


        foreach (var token in tokens)
        {
            if (commandsInject.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 4);


                // "/savechalk": 
                yield return new ConstantToken(new StringVariant("/savechalk"));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);
                // if (breakdown.size() - line_index) < 3: Network._update_chat("Usage: /savechalk <canvas ID> <filename>")
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("breakdown");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("line_index");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpLess);
                yield return new ConstantToken(new IntVariant(3));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Usage: /savechalk <canvas ID> <filename>"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 5);
                // var chalkId = breakdown[line_index + 1]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkId");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("breakdown");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("line_index");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 5);
                // var chalkFilename = breakdown[line_index + 2]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkFilename");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("breakdown");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("line_index");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 5);
                // PlayerData.emit_signal("_chalk_save", chalkId, chalkFilename )
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("emit_signal");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chalk_save"));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("chalkId");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("chalkFilename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 5);
                // return
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 4);

                //  "/helpchalk":
                yield return new ConstantToken(new StringVariant("/helpchalk"));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);
                // Network._update_chat("Usage: /listchalk - lists chalk files, /loadchalk <filename> - loads specific file, /savechalk <canvas ID> <filename> - saves specified canvas with filename")
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Usage: /listchalk - lists chalk files, /loadchalk <filename> - loads specific file, /savechalk <canvas ID> <filename> - saves specified canvas with filename"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 5);
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 4);

                //  "/loadchalk":
                yield return new ConstantToken(new StringVariant("/loadchalk"));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);
                // if (breakdown.size() - line_index) < 2: Network._update_chat("Usage: /loadchalk <filename>")
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("breakdown");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("line_index");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpLess);
                yield return new ConstantToken(new IntVariant(2));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("Network");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_update_chat");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Usage: /loadchalk <filename>"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 5);
                // var chalkFilename = breakdown[line_index + 1]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("chalkFilename");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("breakdown");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("line_index");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 5);
                // PlayerData.emit_signal("_chalk_load", chalkFilename)
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("emit_signal");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chalk_load"));
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("chalkFilename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 5);
                // return
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 4);


                // "/listchalk": PlayerData.emit_signal("_chalk_list")
                yield return new ConstantToken(new StringVariant("/listchalk"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("emit_signal");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chalk_list"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 4);
            }
            else yield return token;
        }
    }
}

