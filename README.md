# Wryte

Wryte is a Blazor WASM application designed for writers, accessible at [wryte.app](https://wryte.app). It provides a simple, serverless environment where you can start creating your novel immediately without the need for accounts or authentication. 

## What is Wryte

Wryte is a writing tool that allows you to create novels, chapters, scenes, and other world-building elements such as locations and items. All elements in Wryte are managed as collections of related content, known as groups. The application uses IndexedDB for storage, ensuring that your data is stored locally on your device.

## Key Features

- **Serverless**: No need authentication, or accounts. Your data is stored locally using IndexedDB.
- **Immediate Access**: Land on the home page and start creating your novel right away.
- **Hierarchical Structure**: Create a novel and organize it into chapters and scenes. You can also add other elements like locations and items for world-building.
- **Flexible Content Organization**: Manage all your content using the `ItemModel` class, which supports parent-child relationships for flexible organization.
- **Event Association**: Associate events with novels, chapters, and scenes to outline the main events in your story.
- **Advanced Writing Pane**: Features include autosave, distraction-free writing, dark mode, and typewriter mode.
- **Writing Statistics**: Track your writing habits with charts showing how much, how often, and when you write (e.g., day of the week, time of day).
- **EPUB Export**: Generate a basic EPUB for your novel, including or excluding any chapters you choose, and download it.

## Usage

### Creating a Novel

1. Navigate to [wryte.app](https://wryte.app).
2. Click on "+ New Novel" to start a new project.
3. Organize your novel by adding chapters, scenes, and other elements as needed.

### Writing

1. Use the writing pane to draft your novel. 
2. Enable distraction-free writing, dark mode, or typewriter mode from the settings to enhance your writing experience.
3. Your work is automatically saved to IndexedDB.

### Tracking Progress

1. View writing statistics to analyze your productivity and writing patterns.
2. Use the charts to see data such as word count over time and writing frequency.
3. You can see individual writing statistics for each novel if needed

### Exporting Your Novel

1. Once your novel is ready, go to the home page
2. Depending on whether you are in list or card view, click the Publish icon (list view), or use the dropdown and select Publish (card view)
3. Provide the title, subtitle and author of the novel
4. Select which chapters to include in the generated EPUB
5. Click Generate and download your very own novel

**Note:** You currently need to have a group called "Chapters" in order to export your novel to EBUB. 

## Why Wryte

Wryte was inspired by other paid-for writing applications. It was created to provide a free, accessible, and fun alternative for writers who want to focus on their craft without the need for complicated setups or subscriptions.
