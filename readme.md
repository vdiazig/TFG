# **[Nombre del Proyecto] - Prototipo Inicial**

## **Descripción General**

El proyecto desarrolla un videojuego educativo enfocado en la concienciación sobre la contaminación, el reciclaje y su potencial desarrollo. El jugador asume el rol de un héroe que explora un mundo abarrotado por los desechos y la contaminación que ha generado nuevas formas de vida a partir de esta. Mediante la recolección y el reciclaje de materiales, el jugador debe crear nuevas herramientas para combatir a los enemigos y progresar en el juego.

La propuesta incluye tres estilos visuales: un entrono 2D para la gestión de recursos, entorno 2.5D para la exploración de las zonas contaminadas y entorno 3D para el combate contra enemigos principales. Esta combinación busca optimizar el rendimiento en dispositivos móviles y ofrecer una experiencia dinámica.

La metodología de desarrollo sigue un enfoque iterativo, usando como motor gráfico Unity, y PlayFab como backend para la gestión de datos y monetización. El proyecto se divide en las fases de conceptualización, diseño, implementación, testing y publicación. También se planifica la presencia digital mediante una landing page y la creación de redes sociales con el objetivo de construir una comunidad de usuarios.

Los resultados esperados son una demo funcional que incluya el área para la gestión de recursos, varias zonas de exploración y el enfrentamiento con un enemigo principal. Las conclusiones se centrarán en la viabilidad técnica en dispositivos móviles, el potencial de integrar múltiples dimensiones jugables y su experiencia visual, y la capacidad para concienciar y educar sobre la problemática elegida.


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

  - **Sistema de notificaciones**:
  - Sistema para la gestión de notificaciones en todo el juego con 3 tipos diferentes:
      - A pantalla completa para mensajes relevantes del juego.
      - En la zona superior izquierda para nuevos items adquiridos.
      - En la zona superior para mensajes del sistema.

- **Sistema de Gestión de Escenas**:
  - Carga y transición entre distintas zonas.

- **Sistema de Gestión y carga de items mediante prefabs**:
  - Carga y transición entre distintas zonas.

- **Pantalla de título**:
  - Lógica de la pantalla de título.

- **Mapa de exploración**:
  - Implementación del mapa de juego con áreas definidas.
  - Zonas delimitadas para futuras expansiones (ciudad, playa, pueblos).

- **Pruebas de Juego en Dispositivos Móviles**:
  - Primer prototipo con funciones básicas en formato **.apk**.

---

## **Enlaces Importantes**

- **Repositorio del Proyecto en GitHub**:  
  [Repositorio en GitHub](https://github.com/vdiazig/TFG.git)

- **Video de Presentación en YouTube**:  
  [Ver Video de Presentación](https://youtu.be/F7w3JZcP7OY)

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

## **Próximos Pasos**

1. Fase 1:
- **Zona de exploración**: Incluir la zona jugable de la primera area de exploración.
- **Personaje**: Añadir el personaje de juego, sus acciones y controles.
- **Interacciones**: Añadir enemigos, objetos de recolección y sistemas de combate.
- **Gestión de recursos**: Comenzar la implementación del area de gestión de recursos.
- **Optimización**: Mejorar el rendimiento en dispositivos móviles.

2. Fase 2:
- **Gestión de recursos**: Finalizar el área de gestión de recursos.
- **Enemigo principal**: Mecánicas del enemigo principal.
- **Optimización**: Mejorar el rendimiento en dispositivos móviles.
- **Marketing**: Redes sociales y Landing Page.




