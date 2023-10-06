using ShopFloor.EFModels;
using ShopFloor.RunTimeClass;
using System.Collections.Generic;
using static DevExpress.Data.Helpers.SyncHelper.ZombieContextsDetector;

namespace ShopFloor.Service
{
	public class TagService
	{
		//private readonly ShopFloorDBContext _DBcontext;
		private readonly IServiceScopeFactory scopeFactory;
		public TagService(IServiceScopeFactory scopeFactory)
		{
			this.scopeFactory = scopeFactory;
			//this._DBcontext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ShopFloorDBContext>();
		}


		#region Tag

		private bool update = true;
		public bool GetUpdateStates()
		{
			return update;
		}
		public void SetUpdateStates(bool flag)
		{
			update = flag;
		}

		private List<TagClass> _tags;
		public Task<List<TagClass>> GetTagClasses()
		{
			return Task.FromResult(_tags);
		}
		public async Task InitTags()
		{
            _tags = (await GetTags()).Select(x => new TagClass(x)).ToList();
		}
        public Task<List<Tag>> GetTags()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
                return Task.FromResult(context.Tags.ToList());
            }
        }
        public event Action<List<TagClass>>? AllTagslAct;
		private void OnAllTagsChanged(List<TagClass> tags) => AllTagslAct?.Invoke(_tags);

		public event Action<TagClass>? SingleTagUpsertlAct;
		private void OnSingleTagUpsert(TagClass tag) => SingleTagUpsertlAct?.Invoke(tag);

		public event Action<TagClass>? SingleTagDeletelAct;
		private void OnSingleTagDelete(TagClass tag) => SingleTagDeletelAct?.Invoke(tag);

		public async Task UpsertTag(Tag newTag)
		{
			await Task.Run(async () =>
			{
				using (var scope = scopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
					IEnumerable<string> tagNames = _tags.Select(x => x.TagName);
					TagClass tmp = new TagClass(newTag);
					//update
					if (tagNames.Contains(newTag.TagName))
					{
						//memory
						TagClass target_memory = _tags.FirstOrDefault(x => x.TagName == newTag.TagName);
						if (target_memory != null)
						{
							target_memory.tagType = tmp.tagType;
							target_memory.value = tmp.value;
							target_memory.lastestUpdate = tmp.lastestUpdate;
						}
						//db
						Tag target_db = context.Tags.FirstOrDefault(x => x.TagName == newTag.TagName);
						if (target_db != null)
						{
							if (target_db.TagType != newTag.TagType)
							{
								target_db.TagType = newTag.TagType;
							}
							else
							{
								return;
							}
						}
						else
						{
							return;
						}
					}
					//insert
					else
					{
						await context.Tags.AddAsync(newTag);
						_tags.Add(tmp);
					}
					await context.SaveChangesAsync();
					OnSingleTagUpsert(tmp);
				}
			});
		}

		public async Task<bool> DeleteTag(Tag newTag)
		{
			bool res = false;
			await Task.Run(async () =>
			{
				using (var scope = scopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
					IEnumerable<string> tagNames = _tags.Select(x => x.TagName);
					//update
					if (tagNames.Contains(newTag.TagName))
					{
						Tag target = context.Tags.FirstOrDefault(x => x.TagName == newTag.TagName);
						if (target != null)
						{
							context.Tags.Remove(target);
							await context.SaveChangesAsync();
							_tags.Remove(_tags.FirstOrDefault(x => x.TagName == newTag.TagName));

							res = true;
						}
						else
						{
						}
					}
					//not found
					else
					{}
				}
			});
			if (res)
			{
				//await InitTags();
				OnSingleTagDelete(new TagClass(newTag));
			}
			return res;
		}
		#endregion


	}
}
