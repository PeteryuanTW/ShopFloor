namespace ShopFloor.Service
{
	public class TagUpdateService : BackgroundService
	{
		private readonly TagService _tagService;

		public TagUpdateService(TagService tagService)
		{
			this._tagService = tagService;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _tagService.InitTags();
			while (!stoppingToken.IsCancellationRequested)
			{
				if (_tagService.GetUpdateStates())
				{
					Console.WriteLine("updating...");

				}
				else
				{
					Console.WriteLine("stop updating...");
				}
				await Task.Delay(1000);
			}
		}
	}
}
