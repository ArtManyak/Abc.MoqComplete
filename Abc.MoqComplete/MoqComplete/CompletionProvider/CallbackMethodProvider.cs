﻿using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.AspectLookupItems.BaseInfrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.AspectLookupItems.Info;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExpectedTypes;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.UI.RichText;
using MoqComplete.Extensions;
using System.Linq;

namespace MoqComplete.CompletionProvider
{
    [Language(typeof(CSharpLanguage))]
    public class CallbackMethodProvider : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            var codeCompletionType = context.BasicContext.CodeCompletionType;
            return codeCompletionType == CodeCompletionType.SmartCompletion || codeCompletionType == CodeCompletionType.BasicCompletion;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            var identifier = context.TerminatedContext.TreeNode as IIdentifier;
            var setupExpression = identifier.GetParentSafe<IReferenceExpression>();
            if (setupExpression == null)
                return false;

            if (!(setupExpression.ConditionalQualifier is IInvocationExpression setupInvocation))
                return false;

            var mockedMethod = setupInvocation.GetMockedMethodFromSetupMethod();
            if (mockedMethod == null || mockedMethod.Parameters.Count == 0)
                return false;

            var types = mockedMethod.Parameters.Select(p => p.Type.GetPresentableName(CSharpLanguage.Instance));
            var variablesName = mockedMethod.Parameters.Select(p => p.ShortName);
            var proposedCallback = $"Callback<{string.Join(",", types)}>(({string.Join(",", variablesName)}) => {{}})";
            var item = CSharpLookupItemFactory.Instance.CreateKeywordLookupItem(context, proposedCallback, TailType.None, PsiSymbolsThemedIcons.Method.Id) as LookupItem<TextualInfo>;
            if (item == null)
                return false;

            item.SetInsertCaretOffset(-2);
            item.SetReplaceCaretOffset(-2);
            item.WithInitializedRanges(context.CompletionRanges, context.BasicContext);
            item.PlaceTop();
            collector.Add(item);
            return true;
        }
    }
}
