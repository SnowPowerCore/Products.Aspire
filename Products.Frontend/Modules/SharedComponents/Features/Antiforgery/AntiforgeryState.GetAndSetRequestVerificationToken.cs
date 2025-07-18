using Apizr;
using Products.PublicApi.Api;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Extensions;
using TimeWarp.State;

namespace Products.Frontend.SharedComponents.Features.Antiforgery;

partial class AntiforgeryState
{
    public static class GetAndSetRequestVerificationTokenActionSet
    {
        public sealed class Action : IAction
        {
            public Action() { }
        }

        public sealed class Handler : ActionHandler<Action>
        {
            private readonly IApizrManager<ITokensApi> _tokensApi;

            public Handler(IStore store, IApizrManager<ITokensApi> tokensApi) : base(store)
            {
                _tokensApi = tokensApi;
            }

            private AntiforgeryState AntiforgeryState => Store.GetState<AntiforgeryState>();

            public override async Task Handle(Action action, CancellationToken cancellationToken)
            {
                using var response = await _tokensApi.ExecuteAsync(static (opt, api) =>
                    api.GetAntiforgeryToken(opt), o => o.WithCancellation(cancellationToken));
                var data = response.ToData<AntiforgeryResultDto>(out var errors);
                if (data is default(AntiforgeryResultDto) && errors.Count > 0)
                    return;

                AntiforgeryState.RequestVerificationToken = data!.RequestToken!;
            }
        }
    }
}