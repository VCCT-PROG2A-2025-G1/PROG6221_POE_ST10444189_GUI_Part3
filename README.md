# 🛡️ SecureBot - Cybersecurity Awareness Chatbot

## Overview
SecureBot is an intelligent cybersecurity education chatbot built using C# WPF. The application helps users learn about cybersecurity best practices through interactive conversations, quizzes, and task management while tracking their learning progress.

## Features

### 🤖 Enhanced Natural Language Processing
- **Advanced keyword detection** across 7+ cybersecurity topics
- **Natural language understanding** for task creation and user intents
- **Context-aware responses** based on comprehensive input analysis
- **Sentiment analysis** for emotional context understanding
- **Entity extraction** from user phrases and commands

### 🎯 Interactive Cybersecurity Quiz
- **12+ comprehensive questions** covering key security topics
- **Multiple choice and true/false formats**
- **Immediate feedback** with detailed explanations
- **Score tracking** with performance-based feedback
- **Question shuffling** for variety in each attempt

### 📋 Smart Task Management
- **Natural language task creation** (e.g., "Remind me to update passwords")
- **Automatic cybersecurity descriptions** generated for tasks
- **Reminder system** with customizable dates
- **Task completion and deletion** tracking
- **Task statistics** and progress monitoring

### 📊 Activity Logging & Progress Tracking
- **Comprehensive activity tracking** for all user interactions
- **Paginated activity history** with "Show More" functionality
- **Learning statistics** including tasks completed and quiz scores
- **Conversation topic tracking** for educational insights
- **Automatic activity cleanup** for optimal performance

### 💬 Intelligent Conversation System
- **Topic-specific guidance** on passwords, phishing, privacy, scams, and more
- **Contextual follow-up responses** based on conversation flow
- **Personalization features** (remembers user name and preferences)
- **Multi-turn conversations** with topic continuity

## Technical Architecture

### Core Classes

#### `EnhancedNLP.cs`
- **Keyword Detection**: Comprehensive dictionaries for cybersecurity topics
- **Phrase Pattern Recognition**: Intent detection through natural language patterns
- **Topic Analysis**: Scoring system for accurate topic identification
- **Entity Extraction**: Automated extraction of task names and actions from user input

#### `ActivityLog.cs`
- **Activity Tracking**: Structured logging of all user interactions
- **Pagination Support**: Efficient handling of large activity histories
- **Statistics Generation**: Comprehensive activity and learning statistics
- **Data Management**: Automatic cleanup and memory optimization

#### `ChatBot.cs` 
- **NLP Integration**: Every input processed through advanced language analysis
- **Intelligent Routing**: Priority-based handling of different request types
- **Activity Integration**: Seamless logging of all user interactions
- **Enhanced Task Flow**: Natural language task creation and management

#### `CybersecurityQuiz.cs`
- **Question Management**: 12+ questions with explanations and scoring
- **Progress Tracking**: Real-time score calculation and performance feedback
- **Dynamic Content**: Question shuffling and varied quiz experiences

#### `TaskManager.cs`
- **CRUD Operations**: Complete task lifecycle management
- **Smart Descriptions**: Automatic cybersecurity context generation
- **Statistics**: Comprehensive task analytics and reporting

### Supporting Components
- **CyberTask.cs**: Task data model with reminder and completion tracking
- **CyberTips.cs**: Curated cybersecurity advice and tips
- **AudioPlayer.cs**: Audio feedback system
- **AsciiArt.cs**: Visual enhancement for user experience

## Installation & Setup

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2 or higher
- Windows 10 or later

### Getting Started
1. Clone the repository
2. Open `PROG6221_POE_ST10444189_GUI.sln` in Visual Studio
3. Build the solution (Build → Build Solution)
4. Run the application (F5 or Debug → Start Debugging)

### Optional Resources
- Place `greeting.wav` in `Resources/` folder for audio feedback
- Add `ascii_art.txt` in `Resources/` folder for visual enhancement

## Usage Guide

### Basic Commands
```
"help" - Display all available commands
"My name is [Name]" - Introduce yourself to the bot
"start quiz" - Begin cybersecurity knowledge test
"show my tasks" - View all created tasks
"show activity log" - View learning history
```

### Task Management
```
"Add task - update passwords" - Create a new task
"Remind me to check privacy settings" - Natural language task creation
"complete task 1" - Mark task as completed
"delete task 2" - Remove a task
"task statistics" - View task progress
```

### Learning & Education
```
"Tell me about password security" - Learn about specific topics
"What is phishing?" - Get detailed explanations
"I'm worried about online privacy" - Express concerns for guidance
"more" - Continue current conversation topic
```

### Activity Tracking
```
"show activity log" - View recent activities
"show more activities" - See additional activity history
"activity stats" - Get comprehensive statistics
```

## Cybersecurity Topics Covered

### 🔐 Password Security
- Strong password creation
- Password manager usage
- Two-factor authentication
- Password policy best practices

### 🎣 Phishing Prevention
- Email verification techniques
- Suspicious link identification
- Social engineering awareness
- Reporting procedures

### 🛡️ Privacy Protection
- Data sharing guidelines
- Social media privacy settings
- VPN usage and benefits
- Personal information management

### 🚨 Scam Recognition
- Phone and email scam identification
- Fraud prevention techniques
- Safe online shopping practices
- Identity theft protection

### 🦠 Malware Protection
- Antivirus best practices
- Safe downloading guidelines
- System update importance
- Backup strategies

## Development Features

### Code Quality
- **Consistent commenting style** throughout the codebase
- **Comprehensive error handling** for robust operation
- **Modular architecture** for easy maintenance and expansion
- **Clean separation of concerns** between UI and business logic

### Performance Optimizations
- **Efficient NLP processing** with optimized keyword matching
- **Paginated data handling** for large activity logs
- **Memory management** with automatic cleanup routines
- **Asynchronous operations** for responsive UI

## Future Enhancements
- Data persistence for user progress
- Advanced reporting and analytics
- Multi-language support
- Integration with external security APIs
- Mobile application version

## Contributing
This project is part of an academic assignment. For educational use and reference only.

## License
Academic project - All rights reserved.

## Contact
Student ID: ST10444189  
Course: PROG6221 - Programming 2B  
Institution: [Your Institution Name]

---

*SecureBot - Empowering users with cybersecurity knowledge through intelligent conversation and interactive learning.*
