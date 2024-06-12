using Wryte.Models;
using TG.Blazor.IndexedDB;
using System;

namespace Wryte.Services
{
    public class DatabaseService
    {

        // Constants 


        public const string Novels = "Novels";

        public const string Categories = "Categories";

        public const string Groups = "Groups";

        public const string Events = "Events";

        public const string Items = "Items";


        public const string ItemTags = "ItemTags";

        public const string Sequences = "Sequences";

        public const string Settings = "Settings";

        public const string Sessions = "Sessions";

        public const string Tags = "Tags";

        // Fields

        private readonly IndexedDBManager _manager;

        // Constructors

        public DatabaseService(IndexedDBManager manager) 
        {
            _manager = manager;
        }

        // Methods - Crud for Item models

        public async Task<bool> Create(ItemModel model, string store)
        {
            try
            {
                var record = new StoreRecord<ItemModel>
                {
                    Storename = store,
                    Data = model
                };

                await _manager.AddRecord(record);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<List<ItemModel>> Get(string store)
        {
            try
            {
                return await _manager.GetRecords<ItemModel>(store);
            }
            catch { }

            return null;
        }

        public async Task<ItemModel> Get(Guid id, string store)
        {
            try
            {
                return await _manager.GetRecordById<Guid, ItemModel>(store, id);
            }
            catch { }

            return null;
        }

        public async Task<List<ItemModel>> GetChildren(Guid id, string store)
        {
            var children = new List<ItemModel>();

            var items = await _manager.GetRecords<ItemModel>(store);

            if (items == null)
            {
                return children;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ParentId == id)
                {
                    children.Add(items[i]);
                }
            }

            return children;
        }

        public async Task<bool> Save(ItemModel item, string store)
        {
            try
            {
                item.Updated = DateTime.Now;

                var record = new StoreRecord<ItemModel>
                {
                    Storename = store,
                    Data = item
                };

                await _manager.UpdateRecord(record);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(Guid id, string store)
        {
            try
            {
                await _manager.DeleteRecord(store, id);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Exists(Guid id, string store)
        {
            try
            {
                var item = await _manager.GetRecordById<Guid, ItemModel>(store, id);

                return item != null;
            }
            catch { return false; }
        }


        // Methods - Session CRUD

        #region Session CRUD

        public async Task<bool> Create(SessionModel session)
        {
            try
            {
                var record = new StoreRecord<SessionModel>
                {
                    Storename = Sessions,
                    Data = session
                };
                await _manager.AddRecord(record);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Save(SessionModel session)
        {
            try
            {
                session.End = DateTime.Now;

                var record = new StoreRecord<SessionModel>
                {
                    Storename = Sessions,
                    Data = session
                };

                await _manager.UpdateRecord(record);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        // Methods - Tag CRUD

        #region - Tags CRUD

        public async Task<bool> Delete(ItemModel tag)
        {
            try
            {
                // Delete the novel tag records

                var maps = await _manager.GetRecords<ItemTagModel>(Tags);

                var tagMaps = maps.Where(x => x.TagId == tag.Id).ToList();

                foreach (var tagMap in tagMaps)
                {
                    await _manager.DeleteRecord(ItemTags, tagMap.Id);
                }

                // Delete the tag

                await _manager.DeleteRecord(Tags, tag.Id);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        // Methods - NovelTag

        #region NovelTag CRUD

        public async Task Create(ItemTagModel novelTag)
        {
            var map = new StoreRecord<ItemTagModel>
            {
                Storename = ItemTags,
                Data = novelTag,
            };

            await _manager.AddRecord(map);
        }

        public async Task<List<ItemModel>> GetTags(Guid id)
        {
            try
            {
                var maps = await _manager.GetRecords<ItemTagModel>(ItemTags);

                if (maps == null)
                {
                    return new List<ItemModel>();
                }
                else if (maps.Count == 0)
                {
                    return new List<ItemModel>();
                }

                var tagIds = maps
                    .Where(x => x.ItemId == id)
                    .Select(x => x.TagId)
                    .ToList();

                var tags = await _manager.GetRecords<ItemModel>(Tags);

                var novelTags = new List<ItemModel>();

                foreach (var tag in tags)
                {
                    if (tagIds.Contains(tag.Id))
                    {
                        novelTags.Add(tag);
                    }
                }

                return novelTags;
            }
            catch
            {
                return new List<ItemModel>();
            }
        }

        public async Task<List<ItemTagModel>> GetItemTagMaps(Guid id)
        {
            var maps = await _manager.GetRecords<ItemTagModel>(ItemTags);

            return maps.Where(x => x.ItemId == id).ToList();
        }

        public async Task<bool> Delete(ItemTagModel novelTag)
        {
            try
            {
                await _manager.DeleteRecord(ItemTags, novelTag.Id);
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        // Methods - Settings CRUD

        #region Settings CRUD

        public async Task<bool> Create(SettingsModel settings)
        {
            try
            {
                var record = new StoreRecord<SettingsModel>
                {
                    Storename = Settings,
                    Data = settings
                };

                await _manager.AddRecord(record);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<SettingsModel> GetSettings()
        {
            SettingsModel settings = new SettingsModel();

            try
            {
                var list = await _manager.GetRecords<SettingsModel>(Settings);

                if (list[0] != null)
                {
                    settings = list[0];
                }
                else
                {
                    await Save(settings);
                }
            }
            catch { }

            return settings;
        }

        public async Task<bool> Save(SettingsModel settings)
        {
            try
            {
                var record = new StoreRecord<SettingsModel>
                {
                    Storename = Settings,
                    Data = settings
                };

                await _manager.UpdateRecord(record);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        // Methods - Session statistics

        public async Task<List<SessionModel>> GetAllSessions()
        {
            try
            {
                return await _manager.GetRecords<SessionModel>(Sessions);
            }
            catch
            {
                return new List<SessionModel>();
            }
        }

        public async Task<List<SessionModel>> GetNovelSessions(Guid novelId)
        {
            List<SessionModel> sessions;

            try
            {
                sessions = await _manager.GetRecords<SessionModel>(Sessions);
            }
            catch
            {
                return new List<SessionModel>();
            }

            return sessions.Where(x => x.NovelId == novelId).ToList();
        }


        public async Task<List<SessionModel>> GetItemSessions(Guid id)
        {
            List<SessionModel> sessions;
            try
            {
                sessions = await _manager.GetRecords<SessionModel>(Sessions);
            }
            catch
            {
                return new List<SessionModel>();
            }

            return sessions.Where(x => x.SceneId == id).ToList();
        }

    }
}
