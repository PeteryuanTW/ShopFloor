using ShopFloor.EFModels;
using ShopFloor.RunTimeClass;
using System.Collections.Generic;
using System.Formats.Asn1;
using static DevExpress.Data.Helpers.SyncHelper.ZombieContextsDetector;
//using TagClass = ShopFloor.RunTimeClass.TagClass;

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

		private List<TagSuperClass> _tags;
		public Task<List<TagSuperClass>> GetTagClasses()
		{
			return Task.FromResult(_tags);
		}
		public async Task InitTags()
		{
			_tags = new();
			List<Tag> tmp = await GetTags();
			foreach (Tag tag in tmp)
			{
				_tags.Add(InitTagByType(tag));
				Console.WriteLine(InitTagByType(tag) is TagBool);
			}
		}
		private TagSuperClass InitTagByType(Tag tag)
		{
			switch (tag.TagType)
			{
				case 0:
					return new TagBool(tag);
				case 1:
					return new TagInt(tag);
				case 2:
					return new TagFloat(tag);
				case 3:
					return new TagString(tag);
				default:
					return new TagSuperClass(tag);
			}
		}
		public Task<List<Tag>> GetTags()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
                return Task.FromResult(context.Tags.ToList());
            }
        }
        public event Action<List<TagSuperClass>>? AllTagslAct;
		private void OnAllTagsChanged(List<TagSuperClass> tags) => AllTagslAct?.Invoke(_tags);

		public event Action<TagSuperClass>? SingleTagUpsertlAct;
		private void OnSingleTagUpsert(TagSuperClass tag) => SingleTagUpsertlAct?.Invoke(tag);

		public event Action<TagSuperClass>? SingleTagDeletelAct;
		private void OnSingleTagDelete(TagSuperClass tag) => SingleTagDeletelAct?.Invoke(tag);

		public async Task UpsertTag(Tag newTag)
		{
			await Task.Run(async () =>
			{
				using (var scope = scopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<ShopFloorDBContext>();
					IEnumerable<string> tagNames = _tags.Select(x => x.TagName);
					//TagSuperClass tmp;
					//update
					if (tagNames.Contains(newTag.TagName))
					{
						//memory
						TagSuperClass target_memory = _tags.FirstOrDefault(x => x.TagName == newTag.TagName);
						if (target_memory != null)
						{
							_tags.Remove(target_memory);
							_tags.Add(InitTagByType(newTag));
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
						OnSingleTagUpsert(target_memory);
					}
					//insert
					else
					{
						TagSuperClass _tmp = InitTagByType(newTag);
						await context.Tags.AddAsync(newTag);
						_tags.Add(_tmp);
						OnSingleTagUpsert(_tmp);
					}
					await context.SaveChangesAsync();
					
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
				OnSingleTagDelete(new TagSuperClass(newTag));
			}
			return res;
		}
		#endregion


	}
}
