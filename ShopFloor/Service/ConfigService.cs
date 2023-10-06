using ShopFloor.EFModels;

namespace ShopFloor.Service
{
	public class ConfigService
	{
		private readonly IServiceScopeFactory scopeFactory;
		public ConfigService(IServiceScopeFactory scopeFactory)
		{
			this.scopeFactory = scopeFactory;
		}

		public Task<List<ActionConfig>> GetActionConfigs()
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
				List<ActionConfig> res = context.ActionConfigs.ToList();
				return Task.FromResult(res);
			}
		}
		public Task<List<ProcessStep>> GetProcessSteps()
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
				List<ProcessStep> res = context.ProcessSteps.ToList();
				return Task.FromResult(res);
			}
		}
		public Task<List<string>> GetProcessName()
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
				List<string> res = context.ProcessSteps.Select(x => x.ProcessName).Distinct().ToList();
				return Task.FromResult(res);
			}
		}
		public Task<List<ProcessStep>> GetProcessStepsByName(string processName)
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
				List<ProcessStep> res = context.ProcessSteps.Where(x=>x.ProcessName == processName).ToList();
				return Task.FromResult(res);
			}
		}
	}
}
