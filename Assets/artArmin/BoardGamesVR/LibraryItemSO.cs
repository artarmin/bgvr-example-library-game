namespace ArtArmin.BoardGamesVR
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "MyLibraryItem", menuName = "Board Games VR/Library Item", order = 1)]
    public sealed class LibraryItemSO : ScriptableObject
    {
        public LibraryItemManifestModel manifest;
    }

    [Serializable]
    public struct LibraryItemManifestModel
    {
        public string id;
        public string title;
        public string coverImageFileName;
        public BoardConfigModel boardConfig;
        public LogicConfigModel logicConfig;
    }

    [Serializable]
    public struct BoardConfigModel
    {
        public string prefabPath;
        public CheckerPieceItemModel[] checkerPieceCollection;

        public bool IsInvalid()
        {
            return
                string.IsNullOrEmpty(prefabPath) ||
                checkerPieceCollection == null || checkerPieceCollection.Length == 0;
        }
    }

    [Serializable]
    public struct CheckerPieceItemModel
    {
        public CheckerModel.CheckerType type;
        public string prefabPath;
        public int playerId;
        public bool useRandomRotation;
    }

    [Serializable]
    public struct LogicConfigModel
    {
        public enum GameMoveGenerationTypeRule
        {
            /// <summary>
            /// Any game type that is not supported by the internal logic.
            /// </summary>
            Custom,

            /// <summary>
            /// Preset move generation rules for backgammon games.
            /// </summary>
            Backgammon,

            /// <summary>
            /// Preset move generation rules for checkers games.
            /// </summary>
            Checkers,

            /// <summary>
            /// Preset move generation rules for chess games.
            /// </summary>
            Chess,
        }

        public enum GameWinType
        {
            /// <summary>
            /// The game should be limited to one winner and one or more losers. There is no possibility of drawing the game when players have no available move.
            /// </summary>
            OnlyHasWinnerAndLoser,

            /// <summary>
            /// The game does allow drawing the game. This way, when there is no available move possible for either players, there will be no winner and the game is drawn.
            /// </summary>
            AllowDrawWithoutWinnerOrLoser,
        }

        public enum GameWinRule
        {
            /// <summary>
            /// The winner is indicated from a pre-defined list of checker positions. First player that gets their checker piece positions matching any of the custom winning positions, is declared the winner.
            /// </summary>
            CustomWinningPositions,

            /// <summary>
            /// In this case, the winner is the one that moves all their pieces out of the board towards the home field (e.g. Backgammon, Manji, etc.)
            /// </summary>
            FirstPlayerThatMovesAllPiecesFromBoardToHome,

            /// <summary>
            /// The winner is indicated as the player who still has at least one checker piece on the board, when none of the other players have any (e.g. Checkers)
            /// </summary>
            LastPlayerPiecesStandingInBoard,

            /// <summary>
            /// This is meant for chess games and applies the chess rules when indicating a winner. Note that in this mode, draw should be allowed.
            /// </summary>
            ChessWinRule,

        }

        #region Board Setup Parameters

        public int playerCount;

        public bool isDiceAvailable;
        public int diceMinValue;
        public int diceMaxValue;
        public bool diceHasSecondValue;

        public FieldPositionModel fieldPositionsCount;
        public CheckerModel[] checkerPieces;

        #endregion

        #region Game Rule Parmaters

        /// <summary>
        /// A value of zero indicates a random player should be chosen upon start of the game. Any positive number indicates the player id.
        /// </summary>
        public int startingPlayer;

        public bool generateMovesUsingCheckerPiecesOnBoard;
        public bool isPossibleMoveHomeToAny;
        public bool isPossibleMoveAnyToHome;
        public bool isPossibleMoveGraveyardToAny;
        public bool isPossibleMoveAnyToGraveyard;
        public bool isPossibleMoveWithinBoard;

        public bool moveDestinationCanContainCurrentPlayerPieces;
        public bool moveDestinationCanContainOtherPlayerPieces;

        public GameMoveGenerationTypeRule gameMoveGenerationTypeRule;
        public GameWinType gameWinType;
        public GameWinRule gameWinRule;

        /// <summary>
        /// When current player has no moves available (finshed its moves/turn or just cannot make any moves), if no winner can 
        /// be determined by the game win rules yet, check whether the next turn is playable by the next player or not. 
        /// This is done before handing over the turn to the next player. It is useful on a two player game (e.g. Checkers) when the game is 
        /// over due to the move made by the current player, causing the other player to be locked. Or on a game where the game 
        /// can have no winners and ends up in a tie.
        /// </summary>
        public bool gameWinCheckWhetherNextPlayerHasAnyMoves;

        public WinPositionCombination[] gameWinCustomWinningPositions;

        #endregion
    }

    [Serializable]
    public struct FieldPositionModel
    {
        public int x;
        public int y;
        public int z;
    }

    [Serializable]
    public struct WinPositionCombination
    {
        public FieldPositionModel[] positions;
    }

    [Serializable]
    public struct CheckerModel
    {
        public enum CheckerType : byte
        {
            NORMAL,
            KING,
            CHESS_PAWN,
            CHESS_KNIGHT,
            CHESS_BISHOP,
            CHESS_ROOK,
            CHESS_QUEEN,
            CHESS_KING,
        }

        public CheckerType Type;
        public int PlayerId;
        public CheckerIdModel Id;
    }

    [Serializable]
    public struct CheckerIdModel
    {
        public FieldPositionModel startPosition;
        public int index;
    }
}