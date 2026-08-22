// Copyright (c) 2026 Dr. Masroor Ehsan

using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Avalonia.Headless.NUnit;

using AwesomeAssertions;

using ICSharpCode.ILSpy;
using ICSharpCode.ILSpy.AI;
using ICSharpCode.ILSpy.AppEnv;

using NUnit.Framework;

namespace ICSharpCode.ILSpy.Tests;

[TestFixture]
public class AIChatPaneModelTests
{
	[AvaloniaTest]
	public async Task HelpCommand_AppendsCommandOverview()
	{
		AIChatPaneModel pane = CreatePane();

		pane.Input = "/help";
		await pane.SendCommand.ExecuteAsync(null);

		pane.Messages.Last().IsAssistant.Should().BeTrue();
		pane.Messages.Last().Content.Should().Contain("Commands:");
		pane.Input.Should().BeEmpty();
		pane.StatusMessage.Should().Be("Ready");
	}

	[AvaloniaTest]
	public async Task UnknownCommand_AppendsUnsupportedMessage()
	{
		AIChatPaneModel pane = CreatePane();

		pane.Input = "/doesnotexist";
		await pane.SendCommand.ExecuteAsync(null);

		pane.Messages.Last().Content.Should().Contain("Unsupported command '/doesnotexist'");
		pane.StatusMessage.Should().Be("Unknown command");
	}

	[AvaloniaTest]
	public async Task ExplainCommand_WithoutSelection_AppendsGuidance()
	{
		AIChatPaneModel pane = CreatePane();

		pane.Input = "/explain";
		await pane.SendCommand.ExecuteAsync(null);

		pane.Messages.Last().Content.Should().Contain("/explain requires a selected type, method, property, or field");
		pane.IsBusy.Should().BeFalse();
		pane.StatusMessage.Should().Be("Command complete");
	}

	[AvaloniaTest]
	public async Task RenameCommand_WithoutSelection_AppendsGuidance()
	{
		AIChatPaneModel pane = CreatePane();

		pane.Input = "/rename";
		await pane.SendCommand.ExecuteAsync(null);

		pane.Messages.Last().Content.Should().Contain("/rename requires a selected type, method, property, or field");
		pane.IsBusy.Should().BeFalse();
		pane.StatusMessage.Should().Be("Command complete");
	}

	[AvaloniaTest]
	public async Task SendCommand_AttachesAssistantPlaceholderToActiveConversation()
	{
		var settings = AppComposition.Current.GetExport<SettingsService>().AISettings;
		settings.PrivacyConsentAccepted = true;
		settings.ApiKey = string.Empty;
		var profile = settings.ActiveProfile;
		profile.ProviderType = "ollama";
		profile.BaseUrl = "http://127.0.0.1:1";
		profile.Models.Clear();
		profile.Models.Add("test-model");
		profile.LastSelectedModel = "test-model";

		AIChatPaneModel pane = CreatePane();
		pane.NewConversationCommand.Execute(null);
		pane.Input = "hello";
		await pane.SendCommand.ExecuteAsync(null);

		pane.Messages.Last().IsAssistant.Should().BeTrue();
		var history = (ChatHistory)typeof(AIChatPaneModel)
			.GetField("loadedHistory", BindingFlags.Instance | BindingFlags.NonPublic)!
			.GetValue(pane)!;
		history.ActiveConversation.Messages.Last().Should().BeSameAs(pane.Messages.Last());
	}

	static AIChatPaneModel CreatePane()
	{
		var pane = AppComposition.Current.GetExport<AIChatPaneModel>();
		pane.ClearCommand.Execute(null);
		return pane;
	}
}
