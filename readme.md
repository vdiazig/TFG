# **TFG Videojuego**

## **Descripción General**

El proyecto desarrolla un videojuego educativo enfocado en la concienciación sobre la contaminación, el reciclaje y su potencial desarrollo. El jugador asume el rol de un héroe que explora un mundo abarrotado por los desechos y la contaminación que ha generado nuevas formas de vida a partir de esta. Mediante la recolección y el reciclaje de materiales, el jugador debe crear nuevas herramientas para combatir a los enemigos y progresar en el juego.

La propuesta incluye tres estilos visuales: un entrono 2D para la gestión de recursos, entorno 2.5D para la exploración de las zonas contaminadas y entorno 3D para el combate contra enemigos principales. Esta combinación busca optimizar el rendimiento en dispositivos móviles y ofrecer una experiencia dinámica.

La metodología de desarrollo sigue un enfoque iterativo, usando como motor gráfico Unity, y PlayFab como backend para la gestión de datos y monetización. El proyecto se divide en las fases de conceptualización, diseño, implementación, testing y publicación. 

Los resultados esperados son una demo funcional que incluya el área para la gestión de recursos, zona de exploración y el enfrentamiento con un enemigo principal. Las conclusiones se centrarán en la viabilidad técnica en dispositivos móviles, el potencial de integrar múltiples dimensiones jugables y su experiencia visual, y la capacidad para concienciar y educar sobre la problemática elegida.


---
## **Enlaces Importantes**

- **Repositorio del Proyecto en GitHub**:  
  [Repositorio en GitHub](https://github.com/vdiazig/TFG.git)

- **Video de Presentación en YouTube**:  
  [Ver Video de Presentación](https://youtu.be/LmymTMK7em8)

- **Descargar Proyecto Completo (incluyendo carpetas temporales. Permiso de lectura para cuentas de la UOC)**:  
  [Descargar Proyecto](https://drive.google.com/drive/folders/1jUVGiyX3MGTcygm-2j9UK0tRGyxM4DO_?usp=sharing)

---

## **Instrucciones de Instalación**

### **Descargar y Probar el APK**
1. Descarga el archivo **.apk** desde el repositorio (En carpeta Game - Releases).
2. Transfiere el archivo a un dispositivo Android.
3. Instálar permitiendo origenes desconocidos.
4. Datos de juego (sin comillas). *Se permite el registro de nuevos usuarios:
   - Usuario: "user"
   - Password: "password"

### **Cargar Proyecto en Unity**
1. Clona el repositorio de GitHub.
2. Abre el proyecto en Unity (versión 2022.3.46f1).Versión SDK PlayFab 2.205.241108. 
3. Explora las escenas incluidas y realiza modificaciones según sea necesario.

---

## **Características Implementadas**

- **Arquitectura del sistema**:
  - Se implementa una arquitectura limpia dividida en:
    1. Entities.
    2. Infraestructure.
    3. InterfaceAdpaters.
    4. UseCases.

- **Datos usuario**:
  - Implementación de sistema para inicio de sesión y registro de usuario en PlayFab.
  - Script para la gestión de los datos del usuario.

- **Sistema de gestión de datos**:
  - Implementación de sistema para la carga y guardado de ítems en PlayFab.

- **Sistema de notificaciones**:
- Sistema para la gestión de notificaciones en todo el juego con 3 tipos diferentes:
    - A pantalla completa para mensajes relevantes del juego.
    - En la zona superior derecha para nuevos items adquiridos.
    - En la zona superior para mensajes del sistema.

- **Sistema de Gestión de Escenas**:
  - Carga y transición entre distintas zonas.

- **Sistema de Gestión y carga de objetos mediante prefabs**:
  - Carga y pueda a disposición del sistema todos los prefabs disponibles.

- **Controlador de personaje y estado de animaciones**:
  - Controladora de personajes mediante Joystick sensitivo y 2 botones de acción que detectan toca corto y pulsación larga.

- **Mapa de exploración**:
  - Implementación del mapa de juego con áreas definidas.
  - Zonas delimitadas para futuras expansiones (ciudad, playa, pueblos).

- **Pantalla de título**:
  - Lógica de la pantalla de título.

- **Pantalla del área de gestión de recursos**:
  - Lógica básica del área de gestión. Permite consultar los ítems obtenidos, ver los datos del jugador y el mapa de juego.

- **Escena 3D para el enemigo principal**:
  - Implementación del área 3D sin enemigos.

- **Pruebas de Juego en Dispositivos Móviles**:
  - Segundo prototipo con funciones básicas en formato **.apk**.

---

## **Próximos Pasos**

1. Fase 1:
- **Zona de exploración**: Incluir la zona jugable de la primera area de exploración.
- **Personaje**: Añadir el personaje de juego, sus acciones y controles.
- **Interacciones**: Añadir enemigos, objetos de recolección y sistemas de combate.
- **Gestión de recursos**: Comenzar la implementación del area de gestión de recursos.
- **Optimización**: Mejorar el rendimiento en dispositivos móviles.

2. Fase 2:
- **Gestión de recursos**: Área de gestión de recursos.
- **Ítems e interacciones**: Coleccionables y objetos de interacción
- **Área de exploración 1**: Área de exploración con ítems e interacciones.
- **Escena 3D**: Área del enemigo principal.

2. Fase 3:
- **Enemigo principal**: Mecánicas del enemigo principal.
- **Optimización**: Mejorar el rendimiento en dispositivos móviles.
- **Testeo**: Estadísticas de rendimiento y testeo.
