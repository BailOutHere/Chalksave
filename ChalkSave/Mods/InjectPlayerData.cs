using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace ChalkSave.Mods;

public class InjectPlayerData : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var signalsInject = new MultiTokenWaiter([
            t => t.Type is TokenType.PrSignal,
            t => t is IdentifierToken { Name: "_inventory_refresh" }
        ]);


        foreach (var token in tokens)
        {
            if (signalsInject.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline);
                //          yield return new Token(TokenType.Newline, 0);
                // signal _chalk_save(canvasId, filename)
                yield return new Token(TokenType.PrSignal);
                yield return new IdentifierToken("_chalk_save");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("canvasId");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline);

                // signal _chalk_load(filename)
                yield return new Token(TokenType.PrSignal);
                yield return new IdentifierToken("_chalk_load");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("filename");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline);

                // signal _chalk_list
                yield return new Token(TokenType.PrSignal);
                yield return new IdentifierToken("_chalk_list");
                yield return new Token(TokenType.Newline);
            }
            else yield return token;
        }
    }
}

