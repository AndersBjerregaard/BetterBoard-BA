using App.Betterboard.Api.Model;
using App.Betterboard.Tests.Unit.Builders.Implementations;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Tests.Unit.Builders.Implementation;

public class DefaultBoardBuilder : BoardBuilder, IDefaultBuilder<Board>
{
    public Board Build()
    {
        this.BuildId();
        this.BuildBoardName();
        this.BuildMeetings();
        this.BuildVAT();
        if (this._board.Members == null)
            this.BuildMembers();
        this.BuildDocuments();
        this.BuildCreated();
        this.BuildCreatedBy();
        this.BuildType();
        this.BuildParent();
        this.BuildOnlineMeetingUrl();
        this.BuildEnableSignFeature();
        this.BuildDocumentSettings();
        this.BuildPollSettings();
        this.BuildDefaultOnlineMeetingUrl();
        return this.GetResult();
    }
}